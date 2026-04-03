using EcomTemplate.Application.DTOs;


namespace EcomTemplate.Application.Interfaces;
public interface IInvoiceService
{
    Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId);
    Task<InvoiceDTO?> GetInvoiceByOrderIdAsync(Guid orderId);
}
