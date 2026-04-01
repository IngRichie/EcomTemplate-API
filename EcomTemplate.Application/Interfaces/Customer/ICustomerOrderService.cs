using GrocerySupermarket.Application.DTOs;


namespace GrocerySupermarket.Application.Interfaces;
public interface IOrderService
{
    Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request, Guid customerId);
    Task<OrderDTO?> GetOrderByIdAsync(Guid orderId);
}
