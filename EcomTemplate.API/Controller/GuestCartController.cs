using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Application.DTOs;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[Route("api/guest-cart")]
public class GuestCartController : ControllerBase
{
    private readonly IGuestCartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GuestCartController(IGuestCartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    // =======================
    // GET GUEST CART
    // =======================
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Guid? guestUserId,
        [FromQuery] string? deviceId)
    {
        var cart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        if (cart == null)
            return NotFound("Guest cart not found.");

        return Ok(_mapper.Map<CartDTO>(cart));
    }

    // =======================
    // ADD ITEM
    // =======================
    [HttpPost("add")]
    public async Task<IActionResult> Add(
        [FromQuery] Guid? guestUserId,
        [FromQuery] string? deviceId,
        [FromQuery] Guid productVariantId,
        [FromQuery] int quantity)
    {
        if (guestUserId == null && string.IsNullOrWhiteSpace(deviceId))
            return BadRequest("Guest identifier is required.");

        var cart = await _cartRepository.GetCartByGuest(guestUserId, deviceId)
                   ?? await _cartRepository.CreateGuestCart(guestUserId, deviceId);

        await _cartRepository.AddToCart(cart.CartId, productVariantId, quantity);

        var updatedCart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        return Ok(_mapper.Map<CartDTO>(updatedCart));
    }

    // =======================
    // UPDATE ITEM
    // =======================
    [HttpPut("update")]
    public async Task<IActionResult> Update(
        [FromQuery] Guid? guestUserId,
        [FromQuery] string? deviceId,
        [FromQuery] Guid productVariantId,
        [FromQuery] int quantity)
    {
        var cart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        if (cart == null)
            return NotFound("Guest cart not found.");

        await _cartRepository.UpdateItemQuantity(cart.CartId, productVariantId, quantity);

        var updatedCart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        return Ok(_mapper.Map<CartDTO>(updatedCart));
    }

    // =======================
    // REMOVE ITEM
    // =======================
    [HttpDelete("remove")]
    public async Task<IActionResult> Remove(
        [FromQuery] Guid? guestUserId,
        [FromQuery] string? deviceId,
        [FromQuery] Guid productVariantId)
    {
        var cart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        if (cart == null)
            return NotFound("Guest cart not found.");

        await _cartRepository.RemoveItem(cart.CartId, productVariantId);

        var updatedCart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        return Ok(_mapper.Map<CartDTO>(updatedCart));
    }

    // =======================
    // CLEAR CART
    // =======================
    [HttpDelete("clear")]
    public async Task<IActionResult> Clear(
        [FromQuery] Guid? guestUserId,
        [FromQuery] string? deviceId)
    {
        var cart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        if (cart == null)
            return NotFound("Guest cart not found.");

        await _cartRepository.ClearCart(cart.CartId);

        var updatedCart = await _cartRepository.GetCartByGuest(guestUserId, deviceId);
        return Ok(_mapper.Map<CartDTO>(updatedCart));
    }
}
