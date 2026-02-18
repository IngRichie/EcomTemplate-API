using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/guest/checkout")]
    public class GuestCheckoutController : ControllerBase
    {
        private readonly IGuestCheckoutService _checkoutService;

        public GuestCheckoutController(IGuestCheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("preview")]
        public async Task<IActionResult> Preview([FromBody] CheckoutRequestDTO request)
        {
            if (!request.GuestUserId.HasValue && string.IsNullOrWhiteSpace(request.DeviceId))
                return BadRequest("GuestUserId or DeviceId is required for guest checkout.");

            try
            {
                var result = await _checkoutService.PreviewCheckoutAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] CheckoutRequestDTO request)
        {
            if (!request.GuestUserId.HasValue && string.IsNullOrWhiteSpace(request.DeviceId))
                return BadRequest("GuestUserId or DeviceId is required for guest checkout.");

            var orderId = await _checkoutService.ConfirmCheckoutAsync(request);
            return Ok(new { orderId });
        }
    }
}
