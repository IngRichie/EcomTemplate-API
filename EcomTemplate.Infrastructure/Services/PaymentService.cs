using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Infrastructure.Repositories;

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
            Amount = order.TotalAmount, // 🔒 ALWAYS from order
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveAsync();

        // Optional: update order status
        order.Status = payment.Status == "success"
            ? "paid"
            : "payment_failed";

        await _orderRepo.SaveAsync();

        return new PaymentDTO
        {
            Id = payment.PaymentId,
            OrderId = payment.OrderId,
            Provider = payment.Provider,
            Amount = payment.Amount,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt,
            ProviderReference = dto.ProviderReference
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
}
