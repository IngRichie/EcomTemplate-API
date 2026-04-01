using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("delivery_address")]
public class DeliveryAddress
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}