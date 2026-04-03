using EcomTemplate.Domain.Entities;


namespace EcomTemplate.Application.Interfaces;
public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice);
    Task<Invoice?> GetByOrderIdAsync(Guid orderId);
}
