using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities
{
    [Table("product_variant_attributes")]
    public class ProductVariantAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductVariantAttributeId { get; set; }

        [Required]
        public Guid ProductVariantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;   // e.g., "Color", "Size"

        [Required]
        [MaxLength(255)]
        public string Value { get; set; } = null!;  // e.g., "Blue", "M"

        // Navigation Property
        [ForeignKey(nameof(ProductVariantId))]
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
