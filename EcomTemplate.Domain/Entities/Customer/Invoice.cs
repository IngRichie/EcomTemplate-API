using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("invoice")]
public class Invoice
{
    [Key] public Guid InvoiceId { get; set; }
    public Guid OrderId { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    [Column(TypeName = "numeric(18,2)")] public decimal Subtotal { get; set; }
    [Column(TypeName = "numeric(18,2)")] public decimal DeliveryFee { get; set; }
    [Column(TypeName = "numeric(18,2)")] public decimal Discount { get; set; }
    [Column(TypeName = "numeric(18,2)")] public decimal Total { get; set; }
    public DateTime IssuedAt { get; set; }
}