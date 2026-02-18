using GrocerySupermarket.Application.DTOs;


namespace GrocerySupermarket.Application.Interfaces;

public interface ICustomerCheckoutService
{
    Task<CheckoutSummaryDTO> PreviewCheckoutAsync(CheckoutRequestDTO request);
    Task<Guid> ConfirmCheckoutAsync(CheckoutRequestDTO request);
}
