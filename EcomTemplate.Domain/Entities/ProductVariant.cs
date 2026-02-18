using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities
{
    [Table("product_variants")]
    public class ProductVariant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductVariantId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Sku { get; set; } = null!;

        [Column(TypeName = "numeric(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        public ICollection<ProductVariantAttribute> Attributes { get; set; }
            = new List<ProductVariantAttribute>();
    }
}
