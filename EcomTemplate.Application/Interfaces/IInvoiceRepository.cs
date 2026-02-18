using GrocerySupermarket.Domain.Entities;


namespace GrocerySupermarket.Application.Interfaces;
public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice);
    Task<Invoice?> GetByOrderIdAsync(Guid orderId);
}
