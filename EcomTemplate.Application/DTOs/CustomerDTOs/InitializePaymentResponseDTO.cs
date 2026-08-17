namespace EcomTemplate.Application.DTOs;

public class InitializePaymentResponseDTO
{
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

public class PaymentVerificationDTO
{
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}
