using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IOrderRepository orderRepository, IMapper mapper)
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

    [HttpGet]
    public async Task<IActionResult> GetMyOrders(Guid customerId)
    {
        var orders = await _orderRepository.GetByCustomerAsync(customerId);
        return Ok(_mapper.Map<List<OrderDTO>>(orders));
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) return NotFound("Order not found");
        return Ok(_mapper.Map<OrderDTO>(order));
    }
}
