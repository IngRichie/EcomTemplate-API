using GrocerySupermarket.Application.Interfaces.Admin;
using GrocerySupermarket.Application.Interfaces.Service.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrocerySupermarket.API.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")] // 🔐 Only Admin can access
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    // =========================
    // REVENUE
    // =========================
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue()
    {
        var result = await _service.GetRevenueStatAsync();
        return Ok(result);
    }

    // =========================
    // ORDERS
    // =========================
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _service.GetOrdersStatAsync();
        return Ok(result);
    }

    // =========================
    // CUSTOMERS
    // =========================
    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await _service.GetCustomersStatAsync();
        return Ok(result);
    }

    // =========================
    // PRODUCTS
    // =========================
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _service.GetProductsStatAsync();
        return Ok(result);
    }
}