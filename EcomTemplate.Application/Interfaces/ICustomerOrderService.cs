using GrocerySupermarket.Application.DTOs;


namespace GrocerySupermarket.Application.Interfaces;
public interface IOrderService
{
    Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request);
    Task<OrderDTO?> GetOrderByIdAsync(Guid orderId);
}
