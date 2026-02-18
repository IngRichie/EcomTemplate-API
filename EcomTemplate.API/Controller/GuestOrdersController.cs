using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/guest/orders")]
public class GuestOrdersController : ControllerBase
{
    private readonly IGuestOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GuestOrdersController(
        IGuestOrderService orderService,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderService = orderService;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDTO request)
    {
        var orderId = await _orderService.CreateOrderFromCheckoutAsync(request);
        return Ok(new { orderId });
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null) return NotFound("Order not found");
        return Ok(order);
    }
}
