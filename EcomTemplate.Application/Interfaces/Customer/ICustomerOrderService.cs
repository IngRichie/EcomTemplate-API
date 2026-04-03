using EcomTemplate.Application.DTOs;


namespace EcomTemplate.Application.Interfaces;
public interface IOrderService
{
    Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request, Guid customerId);
    Task<OrderDTO?> GetOrderByIdAsync(Guid orderId);
}
