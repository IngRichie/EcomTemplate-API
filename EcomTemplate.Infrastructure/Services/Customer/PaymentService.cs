using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using EcomTemplate.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcomTemplate.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly PaystackOptions _options;

    public PaymentService(
        IPaymentRepository paymentRepo,
        IOrderRepository orderRepo,
        AppDbContext dbContext,
        HttpClient httpClient,
        IOptions<PaystackOptions> options)
    {
        _paymentRepo = paymentRepo;
        _orderRepo = orderRepo;
        _dbContext = dbContext;
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto)
    {
        var order = await _orderRepo.GetByIdAsync(dto.OrderId)
            ?? throw new InvalidOperationException("Order not found");

        var existing = await _paymentRepo.GetByOrderIdAsync(order.OrderId);
        if (existing != null)
        {
            return ToDto(existing);
        }

        var payment = new Payment
        {
            OrderId = order.OrderId,
            Provider = "Paystack",
            ProviderReference = GenerateReference(order.OrderId),
            Amount = order.TotalAmount,
            Currency = order.Currency,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveAsync();

        return ToDto(payment);
    }

    public async Task<InitializePaymentResponseDTO> InitializeAsync(InitializePaymentDTO dto, Guid customerId)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new InvalidOperationException("Customer email is required.");

        var order = await _orderRepo.GetByIdAsync(dto.OrderId)
            ?? throw new InvalidOperationException("Order not found");

        if (order.CustomerProfileId != customerId)
            throw new UnauthorizedAccessException("Order does not belong to the authenticated customer.");

        if (order.TotalAmount <= 0)
            throw new InvalidOperationException("Order amount is invalid.");

        if (string.Equals(order.Status, "Paid", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Order is already paid.");

        var secretKey = GetSecretKey();
        var reference = GenerateReference(order.OrderId);
        var callbackUrl = string.IsNullOrWhiteSpace(_options.CallbackUrl) ? null : _options.CallbackUrl;

        var payment = await _paymentRepo.GetByOrderIdAsync(order.OrderId);
        if (payment == null)
        {
            payment = new Payment
            {
                OrderId = order.OrderId,
                Provider = "Paystack",
                CreatedAt = DateTime.UtcNow
            };
            await _paymentRepo.AddAsync(payment);
        }

        payment.ProviderReference = reference;
        payment.Amount = order.TotalAmount;
        payment.Currency = GetCurrency(order);
        payment.Status = "Pending";
        payment.UpdatedAt = DateTime.UtcNow;
        payment.FailureReason = null;

        order.PaymentReference = reference;
        order.Status = "PendingPayment";
        order.UpdatedAt = DateTime.UtcNow;

        await _paymentRepo.SaveAsync();

        using var request = new HttpRequestMessage(HttpMethod.Post, BuildPaystackUrl("/transaction/initialize"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
        request.Content = JsonContent.Create(new
        {
            email = dto.Email,
            amount = ToSubunit(order.TotalAmount),
            currency = payment.Currency,
            reference,
            callback_url = callbackUrl,
            metadata = new
            {
                orderId = order.OrderId,
                customerId
            }
        });

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Paystack initialization failed: {content}");

        var initResponse = JsonSerializer.Deserialize<PaystackInitializeResponse>(
            content,
            JsonOptions()) ?? throw new InvalidOperationException("Invalid Paystack initialization response.");

        if (!initResponse.Status || initResponse.Data == null)
            throw new InvalidOperationException(initResponse.Message ?? "Paystack initialization failed.");

        return new InitializePaymentResponseDTO
        {
            AuthorizationUrl = initResponse.Data.AuthorizationUrl,
            AccessCode = initResponse.Data.AccessCode,
            Reference = initResponse.Data.Reference
        };
    }

    public async Task<PaymentVerificationDTO> VerifyAsync(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new InvalidOperationException("Payment reference is required.");

        var secretKey = GetSecretKey();
        using var request = new HttpRequestMessage(HttpMethod.Get, BuildPaystackUrl($"/transaction/verify/{Uri.EscapeDataString(reference)}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

        using var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Paystack verification failed: {content}");

        var verification = JsonSerializer.Deserialize<PaystackVerifyResponse>(
            content,
            JsonOptions()) ?? throw new InvalidOperationException("Invalid Paystack verification response.");

        if (!verification.Status || verification.Data == null)
            throw new InvalidOperationException(verification.Message ?? "Paystack verification failed.");

        return await ApplyVerificationAsync(verification.Data);
    }

    public async Task ProcessWebhookAsync(string payload, string signature)
    {
        if (!IsValidSignature(payload, signature))
            throw new UnauthorizedAccessException("Invalid Paystack signature.");

        var webhook = JsonSerializer.Deserialize<PaystackWebhook>(payload, JsonOptions())
            ?? throw new InvalidOperationException("Invalid webhook payload.");

        var reference = webhook.Data?.Reference;
        if (string.IsNullOrWhiteSpace(reference))
            throw new InvalidOperationException("Webhook reference is missing.");

        if (string.Equals(webhook.Event, "charge.success", StringComparison.OrdinalIgnoreCase))
        {
            await VerifyAsync(reference);
            return;
        }

        if (webhook.Event?.Contains("fail", StringComparison.OrdinalIgnoreCase) == true)
        {
            var payment = await _paymentRepo.GetByReferenceAsync(reference);
            if (payment == null) return;

            payment.Status = "Failed";
            payment.FailureReason = webhook.Event;
            payment.UpdatedAt = DateTime.UtcNow;
            payment.Order.Status = "PaymentFailed";
            payment.Order.UpdatedAt = DateTime.UtcNow;
            await _paymentRepo.SaveAsync();
        }
    }

    public async Task<PaymentDTO?> GetByOrderIdAsync(Guid orderId)
    {
        var payment = await _paymentRepo.GetByOrderIdAsync(orderId);
        return payment == null ? null : ToDto(payment);
    }

    private async Task<PaymentVerificationDTO> ApplyVerificationAsync(PaystackVerificationData data)
    {
        var payment = await _paymentRepo.GetByReferenceAsync(data.Reference)
            ?? throw new InvalidOperationException("Payment record not found.");

        var order = payment.Order;
        var receivedAmount = FromSubunit(data.Amount);
        var receivedCurrency = data.Currency ?? payment.Currency;

        if (!string.Equals(data.Reference, payment.ProviderReference, StringComparison.Ordinal))
            throw new InvalidOperationException("Payment reference mismatch.");

        if (!string.Equals(receivedCurrency, payment.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Payment currency mismatch.");

        if (receivedAmount != payment.Amount || receivedAmount != order.TotalAmount)
            throw new InvalidOperationException("Payment amount mismatch.");

        if (!string.Equals(data.Status, "success", StringComparison.OrdinalIgnoreCase))
        {
            payment.Status = "Failed";
            payment.FailureReason = data.GatewayResponse;
            payment.UpdatedAt = DateTime.UtcNow;
            order.Status = "PaymentFailed";
            order.UpdatedAt = DateTime.UtcNow;
            await _paymentRepo.SaveAsync();

            return ToVerificationDto(payment);
        }

        if (string.Equals(payment.Status, "Success", StringComparison.OrdinalIgnoreCase) && payment.InventoryFinalized)
            return ToVerificationDto(payment);

        await using var tx = await _dbContext.Database.BeginTransactionAsync();

        payment.Status = "Success";
        payment.ProviderTransactionId = data.Id?.ToString();
        payment.Channel = data.Channel;
        payment.PaidAt = data.PaidAt ?? DateTime.UtcNow;
        payment.UpdatedAt = DateTime.UtcNow;
        payment.FailureReason = null;

        order.Status = "Paid";
        order.PaymentReference = payment.ProviderReference;
        order.UpdatedAt = DateTime.UtcNow;

        if (!payment.InventoryFinalized)
        {
            foreach (var item in order.Items)
            {
                var variant = await _dbContext.ProductVariants
                    .FirstOrDefaultAsync(v => v.ProductVariantId == item.ProductVariantId);

                if (variant == null)
                    throw new InvalidOperationException("Product variant not found during inventory finalization.");

                if (variant.Stock < item.Quantity)
                    throw new InvalidOperationException("Insufficient stock during payment finalization.");

                variant.Stock -= item.Quantity;
            }

            payment.InventoryFinalized = true;
        }

        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();

        return ToVerificationDto(payment);
    }

    private bool IsValidSignature(string payload, string signature)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(GetSecretKey()));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var computed = Convert.ToHexString(hash).ToLowerInvariant();
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computed),
            Encoding.UTF8.GetBytes(signature.ToLowerInvariant()));
    }

    private string GetSecretKey()
    {
        var key = Environment.GetEnvironmentVariable("PAYSTACK_SECRET_KEY");
        if (string.IsNullOrWhiteSpace(key))
            key = _options.SecretKey;

        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("PAYSTACK_SECRET_KEY is missing.");

        return key;
    }

    private string GetCurrency(Order order) =>
        !string.IsNullOrWhiteSpace(order.Currency) ? order.Currency : _options.Currency;

    private string BuildPaystackUrl(string path) =>
        $"{_options.BaseUrl.TrimEnd('/')}{path}";

    private static int ToSubunit(decimal amount) =>
        (int)Math.Round(amount * 100, MidpointRounding.AwayFromZero);

    private static decimal FromSubunit(int amount) => amount / 100m;

    private static string GenerateReference(Guid orderId) =>
        $"ET-{orderId:N}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

    private static PaymentDTO ToDto(Payment payment) => new()
    {
        Id = payment.PaymentId,
        OrderId = payment.OrderId,
        Provider = payment.Provider,
        ProviderReference = payment.ProviderReference,
        Amount = payment.Amount,
        Status = payment.Status,
        CreatedAt = payment.CreatedAt
    };

    private static PaymentVerificationDTO ToVerificationDto(Payment payment) => new()
    {
        Reference = payment.ProviderReference,
        Status = payment.Status,
        OrderId = payment.OrderId,
        Amount = payment.Amount,
        Currency = payment.Currency
    };

    private static JsonSerializerOptions JsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private sealed class PaystackInitializeResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public PaystackInitializeData? Data { get; set; }
    }

    private sealed class PaystackInitializeData
    {
        public string AuthorizationUrl { get; set; } = string.Empty;
        public string AccessCode { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }

    private sealed class PaystackVerifyResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public PaystackVerificationData? Data { get; set; }
    }

    private sealed class PaystackWebhook
    {
        public string? Event { get; set; }
        public PaystackVerificationData? Data { get; set; }
    }

    private sealed class PaystackVerificationData
    {
        public long? Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string? Currency { get; set; }
        public string? Channel { get; set; }
        public string? GatewayResponse { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
