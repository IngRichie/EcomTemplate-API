using GrocerySupermarket.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace GrocerySupermarket.Application.Interfaces
{
    public interface IGuestUserService
    {
        // Returns the GuestUserId for a request, creating a guest user if needed
        Task<Guid> GetOrCreateGuestUserAsync(CheckoutRequestDTO request);
    }
}
