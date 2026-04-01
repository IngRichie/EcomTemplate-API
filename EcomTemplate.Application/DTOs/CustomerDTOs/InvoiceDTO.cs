public class InvoiceDTO
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public DateTime IssuedAt { get; set; }
}
