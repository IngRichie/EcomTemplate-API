using EcomTemplate.Application.DTOs;


namespace EcomTemplate.Application.Interfaces;

public interface ICustomerCheckoutService
{
    Task<CheckoutSummaryDTO> PreviewCheckoutAsync(CheckoutRequestDTO request, Guid customerId);
    Task<Guid> ConfirmCheckoutAsync(CheckoutRequestDTO request, Guid customerId);
}
