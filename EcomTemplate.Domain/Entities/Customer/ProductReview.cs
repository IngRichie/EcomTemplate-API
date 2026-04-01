using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("product_reviews")]
public class ProductReview
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductReviewId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid CustomerProfileId { get; set; }

    [Range(1,5)]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }

    public Product Product { get; set; } = null!;
    [ForeignKey(nameof(CustomerProfileId))]
public CustomerProfile CustomerProfile { get; set; } = null!;

}