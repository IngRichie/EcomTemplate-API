using EcomTemplate.Application.Interfaces;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace EcomTemplate.Infrastructure.Repositories;
public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public InvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _db.Invoices.AddAsync(invoice);
    }

    public async Task<Invoice?> GetByOrderIdAsync(Guid orderId)
    {
        return await _db.Invoices
            .FirstOrDefaultAsync(i => i.OrderId == orderId);
    }
}
