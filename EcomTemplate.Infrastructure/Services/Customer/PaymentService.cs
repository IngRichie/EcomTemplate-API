using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IOrderRepository _orderRepo;

    public PaymentService(
        IPaymentRepository paymentRepo,
        IOrderRepository orderRepo)
    {
        _paymentRepo = paymentRepo;
        _orderRepo = orderRepo;
    }

    public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto)
    {
        var order = await _orderRepo.GetByIdAsync(dto.OrderId)
            ?? throw new InvalidOperationException("Order not found");

        var payment = new Payment
        {
            OrderId = order.OrderId,
            Provider = dto.Provider,
            Amount = order.TotalAmount,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveAsync();

        return new PaymentDTO
        {
            Id = payment.PaymentId,
            OrderId = payment.OrderId,
            Provider = payment.Provider,
            Amount = payment.Amount,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt
        };
    }

    public async Task<PaymentDTO?> GetByOrderIdAsync(Guid orderId)
    {
        var payment = await _paymentRepo.GetByOrderIdAsync(orderId);
        if (payment == null) return null;

        return new PaymentDTO
        {
            Id = payment.PaymentId,
            OrderId = payment.OrderId,
            Provider = payment.Provider,
            Amount = payment.Amount,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt
        };
    }

    // ✅ NEW: Initialize payment (Flutterwave-ready)
    public async Task<string> InitializeAsync(InitializePaymentDTO dto)
    {
        var order = await _orderRepo.GetByIdAsync(dto.OrderId)
            ?? throw new Exception("Order not found");

        // 1️⃣ Create payment record
        var payment = new Payment
        {
            OrderId = order.OrderId,
            Provider = "Flutterwave",
            Amount = order.TotalAmount,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveAsync();

        // 2️⃣ HERE you will call Flutterwave API (later)
        // For now, return a mock URL

        var paymentUrl = $"https://payment-gateway.com/pay/{payment.PaymentId}";

        return paymentUrl;
    }
}