using System;
using System.ComponentModel.DataAnnotations;

namespace GrocerySupermarket.Application.DTOs;

public class CreatePaymentDto
{
    [Required]
    public Guid OrderId { get; set; }

    [Required, StringLength(50)]
    public string Provider { get; set; } = string.Empty;

    public string? ProviderReference { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
