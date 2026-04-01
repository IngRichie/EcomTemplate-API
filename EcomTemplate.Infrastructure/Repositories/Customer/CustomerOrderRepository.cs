using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddOrderAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
    }

    // Explicit interface member
    public async Task AddAsync(Order order)
    {
        await AddOrderAsync(order);  // reuse same logic
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetByCustomerAsync(Guid customerId)
    {
        return await _db.Orders
            .Where(o => o.CustomerProfileId == customerId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.Payment)
            .OrderByDescending(o => o.OrderId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Orders.Update(order);
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
