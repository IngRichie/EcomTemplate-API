namespace EcomTemplate.Infrastructure.Services.Admin;

using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;

public class AddProducts : IAddProducts
{
    private readonly AppDbContext _db;

    public AddProducts(AppDbContext db)
    {
        _db = db;
    }

 public async Task<List<Product>> AddNewProducts(List<Product> products)
{
    var newProducts = new List<Product>();

    foreach (var p in products)
    {
        var product = new Product
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            CategoryId = p.CategoryId,

           Images = p.Images?
    .Select(img => new ProductImage
    {
        ProductImageId = img.ProductImageId,
        ProductId = p.ProductId,
        ImageUrl = img.ImageUrl,
        IsPrimary = img.IsPrimary
    })
    .ToList() ?? new List<ProductImage>(),

            ProductVariants = p.ProductVariants?
    .Select(v => new ProductVariant
    {
        ProductVariantId = v.ProductVariantId,
        ProductId = p.ProductId,
        Sku = v.Sku,
        Price = v.Price,
        Stock = v.Stock,

        Attributes = v.Attributes?
            .Select(a => new ProductVariantAttribute
            {
                ProductVariantAttributeId = a.ProductVariantAttributeId,
                ProductVariantId = v.ProductVariantId,
                Name = a.Name,
                Value = a.Value
            }).ToList()
            ?? new List<ProductVariantAttribute>()
    }).ToList()
    ?? new List<ProductVariant>(),

            Reviews = new List<ProductReview>() // ignore on create
        };

        newProducts.Add(product);
    }

    await _db.Products.AddRangeAsync(newProducts);
    await _db.SaveChangesAsync();

    return newProducts;
}
}