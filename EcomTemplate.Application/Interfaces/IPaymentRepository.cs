using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
    Task<Payment?> GetByIdAsync(Guid paymentId);
    Task AddAsync(Payment payment);

    
    Task SaveAsync();
}
