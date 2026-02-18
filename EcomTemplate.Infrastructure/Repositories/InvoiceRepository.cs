using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GrocerySupermarket.Infrastructure.Repositories;
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
