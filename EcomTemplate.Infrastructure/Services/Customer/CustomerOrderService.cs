using AutoMapper;
using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;

namespace EcomTemplate.Infrastructure.Repositories;

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

    public async Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request, Guid customerId)
    {
       

        var summary = await _checkoutService.PreviewCheckoutAsync(request, customerId);

        var cart = await _cartRepo.GetCartByCustomer(customerId)
                   ?? throw new InvalidOperationException("Customer cart not found.");

        var order = new Order
        {
            CustomerProfileId =  customerId,
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
