using GrocerySupermarket.Application.Interfaces.Admin;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories.Admin;

public class DashboardRespository : IDashboardRespository
{
    private readonly AppDbContext _db;

    public DashboardRespository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<double> GetRevenueAsync(DateTime startDate, DateTime endDate)
    {
        return await _db.Payments
            .Where(x => x.Status == "Success" &&
                        x.CreatedAt >= startDate &&
                        x.CreatedAt < endDate)
            .SumAsync(x => (double?)x.Amount) ?? 0;
    }

    public async Task<int> GetOrdersStatAsync(DateTime startDate, DateTime endDate)
    {
        return await _db.Orders
            .Where(x => x.CreatedAt >= startDate &&
                        x.CreatedAt < endDate)
            .CountAsync();
    }

    public async Task<int> GetCustomersStatAsync(DateTime startDate, DateTime endDate)
    {
        return await _db.CustomerProfiles
            .Where(x => x.CreatedAt >= startDate &&
                        x.CreatedAt < endDate)
            .CountAsync();
    }

    public async Task<int> GetProductsStatAsync(DateTime startDate, DateTime endDate)
    {
        return await _db.Products
            .Where(x => x.CreatedAt >= startDate &&
                        x.CreatedAt < endDate)
            .CountAsync();
    }
}