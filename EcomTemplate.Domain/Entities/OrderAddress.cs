using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("order_addresses")]
public class OrderAddress
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid OrderId { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string AddressLine { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
