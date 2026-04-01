using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

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

    public async Task AddAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
