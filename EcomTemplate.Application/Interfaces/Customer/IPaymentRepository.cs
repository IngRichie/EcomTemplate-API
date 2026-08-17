using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
    Task<Payment?> GetByIdAsync(Guid paymentId);
    Task<Payment?> GetByReferenceAsync(string reference);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    
    Task SaveAsync();
}
