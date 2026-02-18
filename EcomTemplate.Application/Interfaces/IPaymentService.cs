using GrocerySupermarket.Application.DTOs;

namespace GrocerySupermarket.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto);
    Task<PaymentDTO?> GetByOrderIdAsync(Guid orderId);
}
