using EcomTemplate.Application.DTOs;

namespace EcomTemplate.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDTO> CreatePaymentAsync(PaymentDTO dto);
    Task<InitializePaymentResponseDTO> InitializeAsync(InitializePaymentDTO dto, Guid customerId);
    Task<PaymentVerificationDTO> VerifyAsync(string reference);
    Task ProcessWebhookAsync(string payload, string signature);
    Task<PaymentDTO?> GetByOrderIdAsync(Guid orderId);
}
