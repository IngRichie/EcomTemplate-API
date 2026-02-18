using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/checkout")]
    public class CustomerCheckoutController : ControllerBase
    {
        private readonly ICustomerCheckoutService _checkoutService;

        public CustomerCheckoutController(ICustomerCheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("preview")]
        public async Task<IActionResult> Preview([FromBody] CheckoutRequestDTO request)
        {
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
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized("User must be authenticated for customer checkout.");

            var customerId = Guid.Parse(User.FindFirst("customerId")!.Value);
            request.CustomerProfileId = customerId;

            var orderId = await _checkoutService.ConfirmCheckoutAsync(request);
            return Ok(new { orderId });
        }
    }
}
