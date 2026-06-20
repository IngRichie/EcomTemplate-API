using EcomTemplate.Application.DTOs.AdminDTOs.AdminDashboardTables;
using EcomTemplate.Application.Interfaces.Admin.AdminDashboardTables;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcomTemplate.Infrastructure.Repositories.Admin.AdminDashboardTables;


public class DashboardOrderRepository : IDashboardOrderRepository
{
    public readonly AppDbContext _db;

    public DashboardOrderRepository (AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<DashboardOrderDTO>> GetAllOrders(int pageNumber, int pageSize)
    {
        var orders = await _db.Orders.
        OrderByDescending(o => o.CreatedAt).
        Skip((pageNumber -1) * pageSize).
        Take(pageSize).
         Select(o => new DashboardOrderDTO
        {
            OrderId = o.OrderId,

            CustomerName =
                (o.CustomerProfile != null
                    ? $"{o.CustomerProfile.FirstName} {o.CustomerProfile.LastName}"
                    : "Guest Customer"),

            CustomerEmail =
                o.CustomerProfile != null
                    ? o.CustomerProfile.Email
                    : null,

            TotalAmount = o.TotalAmount,

            Status = o.Status,

            CreatedAt = o.CreatedAt
        }).
        ToListAsync();

        return orders;
    }

    public async Task<Order?> GetOrderDetails(Guid orderId)
    {
        var orderDetails = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

        if(orderDetails == null) return null;
        
        return orderDetails;
    }
}