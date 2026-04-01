using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<List<Order>> GetByCustomerAsync(Guid customerId);
    Task<List<Order>> GetAllAsync();
    Task AddAsync(Order order);  
    Task UpdateAsync(Order order);
    Task SaveAsync();
}
