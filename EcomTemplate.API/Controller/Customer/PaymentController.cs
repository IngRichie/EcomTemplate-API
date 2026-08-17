using EcomTemplate.Application.DTOs;
using EcomTemplate.Application.Interfaces;
using EcomTemplate.API.HelperFunctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.WebAPI.Controller;

[ApiController]
[Authorize(Roles = "Customer")]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // =======================
    // CREATE PAYMENT
    // =======================

  [HttpPost]
public async Task<IActionResult> CreatePayment([FromBody] PaymentDTO dto)
{
    try
    {
        if (dto == null)
            return BadRequest(new { error = "Invalid payment data" });

        var result = await _paymentService.CreatePaymentAsync(dto);

        return Ok(result);
    }
    catch 
    {
      
        return StatusCode(500, new
        {
            error = "Something went wrong while processing payment"
        });
    }
}

[HttpPost("initialize")]
public async Task<IActionResult> InitializePayment([FromBody] InitializePaymentDTO dto)
{
    var customerId = UserHelper.GetUserId(User);
    var result = await _paymentService.InitializeAsync(dto, customerId);

    return Ok(result);
}

[AllowAnonymous]
[HttpGet("verify/{reference}")]
public async Task<IActionResult> VerifyPayment(string reference)
{
    var result = await _paymentService.VerifyAsync(reference);
    return Ok(result);
}

[AllowAnonymous]
[HttpPost("webhook")]
public async Task<IActionResult> Webhook()
{
    using var reader = new StreamReader(Request.Body);
    var payload = await reader.ReadToEndAsync();
    var signature = Request.Headers["x-paystack-signature"].ToString();

    await _paymentService.ProcessWebhookAsync(payload, signature);
    return Ok();
}

    // =======================
    // GET PAYMENT BY ORDER
    // =======================

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrder(Guid orderId)
    {
        var payment = await _paymentService.GetByOrderIdAsync(orderId);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }
}
