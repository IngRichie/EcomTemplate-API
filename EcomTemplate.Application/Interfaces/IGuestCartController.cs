using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface IGuestCartRepository
{
    // Get guest cart by guest identifier
    Task<Cart?> GetCartByGuest(Guid? guestUserId, string? deviceId);

    // Create guest cart
    Task<Cart> CreateGuestCart(Guid? guestUserId, string? deviceId);

    // Cart operations
    Task AddToCart(Guid cartId, Guid productVariantId, int quantity);
    Task UpdateItemQuantity(Guid cartId, Guid productVariantId, int quantity);
    Task RemoveItem(Guid cartId, Guid productVariantId);
    Task ClearCart(Guid cartId);
}
