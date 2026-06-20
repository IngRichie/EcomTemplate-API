using EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;
using EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class DashboardCustomerRepository : IDashboardCustomerRepository
{

    public readonly AppDbContext _db;
    public DashboardCustomerRepository(AppDbContext db)
    {
         _db = db;
    }
    public async Task<List<DashboardCustomerDTO>> GetAllCustomers(
    int pageNumber,
    int pageSize
)
{
    var customers = await _db.CustomerProfiles
        .OrderByDescending(c => c.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new DashboardCustomerDTO
        {
            CustomerId = c.CustomerProfileId,

            FullName =
                $"{c.FirstName ?? ""} {c.LastName ?? ""}".Trim(),

            Email = c.Email,

            Phone = c.Phone,

            TotalOrders = c.Orders.Count(),

            TotalSpent = c.Orders.Sum(o =>
                (decimal?)o.TotalAmount) ?? 0,

            JoinedDate = c.CreatedAt,

            Status = "Active"
        })
        .ToListAsync();

    return customers;
}

public async Task<DashboardCustomerDTO?> GetCustomerDetails(
    Guid customerId
)
{
    var customer = await _db.CustomerProfiles
        .Where(c => c.CustomerProfileId == customerId)
        .Select(c => new DashboardCustomerDTO
        {
            CustomerId = c.CustomerProfileId,

            FullName =
                $"{c.FirstName ?? ""} {c.LastName ?? ""}".Trim(),

            Email = c.Email,

            Phone = c.Phone,

            TotalOrders = c.Orders.Count(),

            TotalSpent = c.Orders.Sum(o =>
                (decimal?)o.TotalAmount) ?? 0,

            JoinedDate = c.CreatedAt,

            Status = "Active"
        })
        .FirstOrDefaultAsync();

    return customer;
}
}