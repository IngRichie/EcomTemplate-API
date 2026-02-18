/*using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GrocerySupermarket.Infrastructure.Repositories
{
    public class GuestOrderService : IGuestOrderService
    {
        private readonly IGuestCartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IGuestCheckoutService _checkoutService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GuestOrderService(
            IGuestCartRepository cartRepo,
            IOrderRepository orderRepo,
            IGuestCheckoutService checkoutService,
            AppDbContext db,
            IMapper mapper)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _checkoutService = checkoutService;
            _db = db;
            _mapper = mapper;
        }

        // CREATE ORDER FROM GUEST CHECKOUT
        public async Task<Guid> CreateOrderFromCheckoutAsync(CheckoutRequestDTO request)
        {
            if (!request.GuestUserId.HasValue && string.IsNullOrWhiteSpace(request.DeviceId))
                throw new InvalidOperationException("Guest ID or Device ID is required for guest checkout.");

            // Recalculate totals
            var summary = await _checkoutService.PreviewCheckoutAsync(request);

            // Load guest cart
            var cart = await _cartRepo.GetCartByGuest(request.GuestUserId, request.DeviceId)
                       ?? throw new InvalidOperationException("Guest cart not found.");

            // Create order
            var order = new Order
            {
                GuestUserId = request.GuestUserId,
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

        // GET ORDER DETAILS FOR GUEST
        public async Task<OrderDTO?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            return order == null ? null : _mapper.Map<OrderDTO>(order);
        }
    }
}
*/