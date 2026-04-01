using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Interfaces;
using GrocerySupermarket.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/checkout-settings")]
public class CheckoutSettingsController : ControllerBase
{
    private readonly ICheckoutSettingsRepository _repo;

    public CheckoutSettingsController(ICheckoutSettingsRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] CheckoutSettings dto)
    {
        await _repo.UpdateSettings(dto);
        return Ok("Checkout settings updated");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var settings = await _repo.GetOrCreateSettings();
        return Ok(settings);
    }
}
