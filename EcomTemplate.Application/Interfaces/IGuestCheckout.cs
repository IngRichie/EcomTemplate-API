// IGuestCheckoutService.cs
using GrocerySupermarket.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace GrocerySupermarket.Application.Interfaces
{
    public interface IGuestCheckoutService
    {
        Task<CheckoutSummaryDTO> PreviewCheckoutAsync(CheckoutRequestDTO request);
        Task<Guid> ConfirmCheckoutAsync(CheckoutRequestDTO request);
    }
}
