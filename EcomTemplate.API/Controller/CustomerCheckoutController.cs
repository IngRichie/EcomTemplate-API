using EcomTemplate.API.HelperFunctions;
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
            var customerId = UserHelper.GetUserId(User);
            try
            {
                var result = await _checkoutService.PreviewCheckoutAsync(request, customerId);
                return Ok(result);
            }
            catch
            {
                return BadRequest(new { error = "Unable to preview" });
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] CheckoutRequestDTO request)
        {

            try
            {
                 if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized("User must be authenticated for customer checkout.");

            var customerId = UserHelper.GetUserId(User);
            

            var orderId = await _checkoutService.ConfirmCheckoutAsync(request, customerId);
            return Ok(new { orderId });
            }
            catch 
            {
                
                return BadRequest(new { error = "Invalid checkout request" });
            }
           
        }
    }
}
