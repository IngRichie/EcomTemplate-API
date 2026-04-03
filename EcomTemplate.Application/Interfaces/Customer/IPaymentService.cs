using EcomTemplate.Application.DTOs;

namespace EcomTemplate.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto);
    Task<string> InitializeAsync(InitializePaymentDTO dto);
    Task<PaymentDTO?> GetByOrderIdAsync(Guid orderId);
}
