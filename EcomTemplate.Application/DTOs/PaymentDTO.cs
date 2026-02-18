using System;

namespace GrocerySupermarket.Application.DTOs;

public class PaymentDTO
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string Provider { get; set; } = string.Empty;

    public string? ProviderReference { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
