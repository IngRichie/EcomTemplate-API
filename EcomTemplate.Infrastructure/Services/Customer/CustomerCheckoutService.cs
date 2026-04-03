using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcomTemplate.Infrastructure.Repositories;

public class CustomerCheckoutService : ICustomerCheckoutService
{
    private readonly ICustomerCartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly AppDbContext _dbContext;
    private readonly CheckoutSettings _settings;

    public CustomerCheckoutService(
        ICustomerCartRepository cartRepository,
        IOrderRepository orderRepository,
        AppDbContext dbContext,
        IOptions<CheckoutSettings> settings)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _dbContext = dbContext;
        _settings = settings.Value;
    }

    public async Task<CheckoutSummaryDTO> PreviewCheckoutAsync(CheckoutRequestDTO request, Guid customerId)
    {
  

        var cart = await _cartRepository
            .GetCartByCustomer(customerId);

        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Cart is empty.");

        var subTotal = cart.CartItems.Sum(item =>
            item.ProductVariant.Price * item.Quantity);

        decimal discountAmount = 0;

      
        discountAmount = subTotal * (_settings.ReferralDiscountPercentage / 100);
      

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

    public async Task<Guid> ConfirmCheckoutAsync(CheckoutRequestDTO request, Guid customerId)
    {
        

        // Recalculate totals (security check)
        var summary = await PreviewCheckoutAsync(request, customerId);

        var cart = await _cartRepository
            .GetCartByCustomer(customerId);

        if (cart == null)
            throw new InvalidOperationException("Cart not found.");

        var order = new Order
        {
            CustomerProfileId = customerId,
            TotalAmount = summary.Total,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in cart.CartItems)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.ProductVariant.Price
            });
        }

        await _orderRepository.AddOrderAsync(order);

        cart.IsCheckedOut = true;
        await _dbContext.SaveChangesAsync();

        return order.OrderId;
    }
}
