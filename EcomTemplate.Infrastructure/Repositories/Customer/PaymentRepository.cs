using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcomTemplate.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment?> GetByIdAsync(Guid paymentId)
    {
        return await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task<Payment?> GetByReferenceAsync(string reference)
    {
        return await _dbContext.Payments
            .Include(p => p.Order)
                .ThenInclude(o => o.Items)
            .FirstOrDefaultAsync(p => p.ProviderReference == reference);
    }

    public async Task AddAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }

    public Task UpdateAsync(Payment payment)
    {
        _dbContext.Payments.Update(payment);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
