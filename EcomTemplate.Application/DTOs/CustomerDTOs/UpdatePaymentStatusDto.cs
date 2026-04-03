using System.ComponentModel.DataAnnotations;

namespace EcomTemplate.Application.DTOs;

public class UpdatePaymentStatusDto
{
    [Required, StringLength(20)]
    public string Status { get; set; } = string.Empty;
    // pending | success | failed | refunded
}
