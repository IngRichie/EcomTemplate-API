using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces;

public interface ICustomerCartRepository
{
    Task<Cart> CreateCart(Guid customerId);

    // Get the active cart for the customer
    Task<Cart?> GetCartByCustomer(Guid customerId);

    // Cart operations
    Task AddToCart(Guid customerId, Guid cartId, Guid productVariantId, int quantity);
    Task UpdateItemQuantity(Guid customerId, Guid cartId, Guid productVariantId, int quantity);
    Task RemoveItem(Guid customerId, Guid cartId, Guid productVariantId);
    Task ClearCart(Guid customerId, Guid cartId);
}
