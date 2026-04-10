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
    // Ensure IDs are valid (optional but smart)
    foreach (var product in products)
    {
        if (product.ProductId == Guid.Empty)
            product.ProductId = Guid.NewGuid();

        foreach (var img in product.Images)
        {
            if (img.ProductImageId == Guid.Empty)
                img.ProductImageId = Guid.NewGuid();

            img.ProductId = product.ProductId;
        }

        foreach (var variant in product.ProductVariants)
        {
            if (variant.ProductVariantId == Guid.Empty)
                variant.ProductVariantId = Guid.NewGuid();

            variant.ProductId = product.ProductId;

            foreach (var attr in variant.Attributes)
            {
                if (attr.ProductVariantAttributeId == Guid.Empty)
                    attr.ProductVariantAttributeId = Guid.NewGuid();

                attr.ProductVariantId = variant.ProductVariantId;
            }
        }
    }

    await _db.Products.AddRangeAsync(products);
    await _db.SaveChangesAsync();

    return products;
}
}