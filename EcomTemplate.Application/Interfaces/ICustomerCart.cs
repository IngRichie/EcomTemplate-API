using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface ICustomerCartRepository
{
    Task<Cart> CreateCart(Guid customerId);

    // Get the active cart for the customer
    Task<Cart?> GetCartByCustomer(Guid customerId);

    // Cart operations
    Task AddToCart(Guid cartId, Guid productVariantId, int quantity);
    Task UpdateItemQuantity(Guid cartId, Guid productVariantId, int quantity);
    Task RemoveItem(Guid cartId, Guid productVariantId);
    Task ClearCart(Guid cartId);
}
