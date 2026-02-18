using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class GuestCartRepository : IGuestCartRepository
{
    private readonly AppDbContext _db;

    public GuestCartRepository(AppDbContext db)
    {
        _db = db;
    }

    // =========================
    // GET CART BY GUEST
    // =========================
    public async Task<Cart?> GetCartByGuest(Guid? guestUserId, string? deviceId)
    {
        if (guestUserId == null && string.IsNullOrWhiteSpace(deviceId))
            return null;

        return await _db.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant)
            .FirstOrDefaultAsync(c =>
                !c.IsCheckedOut &&
                ((guestUserId != null && c.GuestUserId == guestUserId) ||
                 (!string.IsNullOrEmpty(deviceId) && c.DeviceId == deviceId)));
    }

    // =========================
    // CREATE GUEST CART
    // =========================
    public async Task<Cart> CreateGuestCart(Guid? guestUserId, string? deviceId)
    {
        var cart = new Cart
        {
            GuestUserId = guestUserId,
            DeviceId = deviceId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Carts.Add(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    // =========================
    // ADD ITEM
    // =========================
    public async Task AddToCart(Guid cartId, Guid productVariantId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        var cart = await _db.Carts.FirstOrDefaultAsync(c => c.CartId == cartId && !c.IsCheckedOut);
        if (cart == null)
            throw new Exception("Cart not found.");

        var variant = await _db.ProductVariants.FirstOrDefaultAsync(v => v.ProductVariantId == productVariantId);
        if (variant == null)
            throw new Exception("Product variant not found.");

        var existingItem = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductVariantId == productVariantId);

        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (newQuantity > variant.Stock)
                throw new Exception("Not enough stock available.");
            existingItem.Quantity = newQuantity;
        }
        else
        {
            if (quantity > variant.Stock)
                throw new Exception("Not enough stock available.");

            var cartItem = new CartItem
            {
                CartId = cartId,
                ProductId = variant.ProductId,
                ProductVariantId = variant.ProductVariantId,
                Quantity = quantity
            };

            _db.CartItems.Add(cartItem);
        }

        await _db.SaveChangesAsync();
    }

    // =========================
    // UPDATE ITEM QUANTITY
    // =========================
    public async Task UpdateItemQuantity(Guid cartId, Guid productVariantId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        var item = await _db.CartItems
            .Include(ci => ci.ProductVariant)
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductVariantId == productVariantId);

        if (item == null)
            throw new Exception("Cart item not found.");

        if (quantity > item.ProductVariant.Stock)
            throw new Exception("Not enough stock available.");

        item.Quantity = quantity;

        await _db.SaveChangesAsync();
    }

    // =========================
    // REMOVE ITEM
    // =========================
    public async Task RemoveItem(Guid cartId, Guid productVariantId)
    {
        var item = await _db.CartItems.FirstOrDefaultAsync(ci =>
            ci.CartId == cartId && ci.ProductVariantId == productVariantId);

        if (item == null)
            throw new Exception("Cart item not found.");

        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
    }

    // =========================
    // CLEAR CART
    // =========================
    public async Task ClearCart(Guid cartId)
    {
        var items = await _db.CartItems
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();

        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }
}
