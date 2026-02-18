using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.WebAPI.Controller;

[ApiController]
[Authorize]
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
        var result = await _paymentService.CreatePaymentAsync(dto);
        return Ok(result);
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
