using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class CustomerOrderService : IOrderService
{
    private readonly ICustomerCartRepository _cartRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly ICustomerCheckoutService _checkoutService;
    private readonly IMapper _mapper;
    private readonly AppDbContext _db;

    public CustomerOrderService(
        ICustomerCartRepository cartRepo,
        IOrderRepository orderRepo,
        ICustomerCheckoutService checkoutService,
        AppDbContext db,
        IMapper mapper)
    {
        _cartRepo = cartRepo;
        _orderRepo = orderRepo;
        _checkoutService = checkoutService;
        _db = db;
        _mapper = mapper;
    }

    public async Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request)
    {
        if (!request.CustomerProfileId.HasValue)
            throw new InvalidOperationException("Customer ID is required for this service.");

        var summary = await _checkoutService.PreviewCheckoutAsync(request);

        var cart = await _cartRepo.GetCartByCustomer(request.CustomerProfileId.Value)
                   ?? throw new InvalidOperationException("Customer cart not found.");

        var order = new Order
        {
            CustomerProfileId = request.CustomerProfileId,
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

        await _orderRepo.AddOrderAsync(order);

        cart.IsCheckedOut = true;
        await _db.SaveChangesAsync();

        return order.OrderId;
    }

    public async Task<OrderDTO?> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        return order == null ? null : _mapper.Map<OrderDTO>(order);
    }
}
