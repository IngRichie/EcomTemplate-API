using GrocerySupermarket.Application.DTOs;


namespace GrocerySupermarket.Application.Interfaces;
public interface IInvoiceService
{
    Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId);
    Task<InvoiceDTO?> GetInvoiceByOrderIdAsync(Guid orderId);
}
