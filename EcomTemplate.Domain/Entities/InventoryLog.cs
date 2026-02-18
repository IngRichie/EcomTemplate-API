using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("inventory_log")]
public class InventoryLog
{
    [Key] public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public int Change { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}