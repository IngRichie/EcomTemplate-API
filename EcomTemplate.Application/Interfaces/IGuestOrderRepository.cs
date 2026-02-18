using GrocerySupermarket.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace GrocerySupermarket.Application.Interfaces
{
    public interface IGuestOrderService
    {
        Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request);
        Task<OrderDTO?> GetOrderByIdAsync(Guid orderId);
    }
}
