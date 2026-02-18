using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class GuestCheckoutService : IGuestCheckoutService
{
    private readonly IGuestCartRepository _cartRepository;
    private readonly AppDbContext _dbContext;
    private readonly CheckoutSettings _settings;
    private readonly IOrderService _orderService;
    private readonly IGuestUserService _guestUserService;

    public GuestCheckoutService(
        IGuestCartRepository cartRepository,
        AppDbContext dbContext,
        IOptions<CheckoutSettings> settings,
        IOrderService orderService,
        IGuestUserService guestUserService)
    {
        _cartRepository = cartRepository;
        _dbContext = dbContext;
        _settings = settings.Value;
        _orderService = orderService;
        _guestUserService = guestUserService;
    }

    // =======================
    // PREVIEW CHECKOUT
    // =======================
    public async Task<CheckoutSummaryDTO> PreviewCheckoutAsync(CheckoutRequestDTO request)
    {
        Cart? cart = null;

        if (request.GuestUserId.HasValue || !string.IsNullOrWhiteSpace(request.DeviceId))
        {
            cart = await _cartRepository.GetCartByGuest(
                request.GuestUserId,
                request.DeviceId
            );
        }
        else
        {
            throw new InvalidOperationException(
                "Guest checkout requires a guest identity (GuestUserId or DeviceId).");
        }

        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Cart is empty.");

        var subTotal = cart.CartItems.Sum(item =>
            item.ProductVariant.Price * item.Quantity);

        decimal discountAmount = 0;

        if (!string.IsNullOrWhiteSpace(request.ReferralCode))
        {
            var referral = await _dbContext.ReferralCodes
                .FirstOrDefaultAsync(r =>
                    r.Code == request.ReferralCode &&
                    (r.ExpiresAt == null || r.ExpiresAt > DateTime.UtcNow));

            if (referral != null)
            {
                discountAmount =
                    referral.DiscountType == DiscountType.Percentage
                        ? subTotal * (referral.DiscountValue / 100)
                        : referral.DiscountValue;
            }
        }
        else
        {
            discountAmount = subTotal * (_settings.ReferralDiscountPercentage / 100);
        }

        var taxableAmount = subTotal - discountAmount;
        var taxAmount = taxableAmount * (_settings.TaxPercentage / 100);

        var deliveryFee = request.UseDelivery ? _settings.DeliveryFee : 0;

        return new CheckoutSummaryDTO
        {
            SubTotal = subTotal,
            Discount = discountAmount,
            Tax = taxAmount,
            DeliveryFee = deliveryFee,
            Total = taxableAmount + taxAmount + deliveryFee
        };
    }

    // =======================
    // CONFIRM CHECKOUT
    // =======================
    public async Task<Guid> ConfirmCheckoutAsync(CheckoutRequestDTO request)
    {
        // Recalculate to prevent tampering
        await PreviewCheckoutAsync(request);

        if (!request.GuestUserId.HasValue)
        {
            var guestUserId = await _guestUserService.GetOrCreateGuestUserAsync(request);
            request.GuestUserId = guestUserId;
        }

        var orderId = await _orderService.CreateOrderFromCheckoutAsync(request);
        return orderId;
    }
}
