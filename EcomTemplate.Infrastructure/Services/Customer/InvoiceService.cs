using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using AutoMapper;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;

    public InvoiceService(
        IInvoiceRepository invoiceRepo,
        IOrderRepository orderRepo,
        IMapper mapper)
    {
        _invoiceRepo = invoiceRepo;
        _orderRepo = orderRepo;
        _mapper = mapper;
    }

    public async Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId)
            ?? throw new InvalidOperationException("Order not found");

       var invoice = new Invoice
        {
            OrderId = order.OrderId,
            Subtotal = order.SubTotal,
            DeliveryFee = order.DeliveryFee,
            Discount = order.DiscountAmount,
            Total = order.TotalAmount,
            IssuedAt = DateTime.UtcNow
        };


        await _invoiceRepo.AddAsync(invoice);

        return _mapper.Map<InvoiceDTO>(invoice);
    }

    public async Task<InvoiceDTO?> GetInvoiceByOrderIdAsync(Guid orderId)
    {
        var invoice = await _invoiceRepo.GetByOrderIdAsync(orderId);
        return invoice == null ? null : _mapper.Map<InvoiceDTO>(invoice);
    }
}
