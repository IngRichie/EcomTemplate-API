using GrocerySupermarket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetInvoice(Guid orderId)
    {
        var invoice = await _invoiceService.GetInvoiceByOrderIdAsync(orderId);
        return invoice == null ? NotFound() : Ok(invoice);
    }
}
