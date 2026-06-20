using EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;
using Microsoft.AspNetCore.Mvc;

namespace EcomTemplate.API.Controller.Admin.AdminDashboardTables;


   [ApiController]
[Route("api/admin/dashboard")]
public class DashboardCustomersController : ControllerBase
{
    private readonly IDashboardCustomerRepository _repo;

    public DashboardCustomersController(
        IDashboardCustomerRepository repo
    )
    {
        _repo = repo;
    }

    [HttpGet("customers/all-customers")]
    public async Task<IActionResult> GetCustomers(
        int pageNumber = 1,
        int pageSize = 20
    )
    {
        var customers = await _repo.GetAllCustomers(
            pageNumber,
            pageSize
        );

        return Ok(customers);
    }

    [HttpGet("customers/{customerId}")]
    public async Task<IActionResult> GetCustomer(
        Guid customerId
    )
    {
        var customer =
            await _repo.GetCustomerDetails(customerId);

        if (customer == null)
        {
            return NotFound(new
            {
                Message = "Customer not found"
            });
        }

        return Ok(customer);
    }
}
