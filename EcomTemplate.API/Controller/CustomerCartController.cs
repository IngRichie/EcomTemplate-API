using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.DTOs;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[Authorize]
[Route("api/cart")]
public class CustomerCartController : ControllerBase
{
    private readonly ICustomerCartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CustomerCartController(ICustomerCartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    // =======================
    // GET CART
    // =======================
    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] Guid customerId)
    {
        var cart = await _cartRepository.GetCartByCustomer(customerId);
        if (cart == null)
            return NotFound("Cart not found.");

        return Ok(_mapper.Map<CartDTO>(cart));
    }

    // =======================
    // CREATE CART
    // =======================
    [HttpPost("create")]
    public async Task<IActionResult> CreateCart([FromQuery] Guid customerId)
    {
        var cart = await _cartRepository.CreateCart(customerId);
        return Ok(_mapper.Map<CartDTO>(cart));
    }

    // =======================
    // ADD ITEM
    // =======================
    [HttpPost("add")]
    public async Task<IActionResult> AddItem(
        [FromQuery] Guid cartId,
        [FromQuery] Guid productVariantId,
        [FromQuery] int quantity)
    {
        await _cartRepository.AddToCart(cartId, productVariantId, quantity);
        return Ok();
    }

    // =======================
    // UPDATE ITEM QUANTITY
    // =======================
    [HttpPut("update")]
    public async Task<IActionResult> UpdateItem(
        [FromQuery] Guid cartId,
        [FromQuery] Guid productVariantId,
        [FromQuery] int quantity)
    {
        await _cartRepository.UpdateItemQuantity(cartId, productVariantId, quantity);
        return Ok();
    }

    // =======================
    // REMOVE ITEM
    // =======================
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveItem(
        [FromQuery] Guid cartId,
        [FromQuery] Guid productVariantId)
    {
        await _cartRepository.RemoveItem(cartId, productVariantId);
        return Ok();
    }

    // =======================
    // CLEAR CART
    // =======================
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart([FromQuery] Guid cartId)
    {
        await _cartRepository.ClearCart(cartId);
        return Ok();
    }
}
