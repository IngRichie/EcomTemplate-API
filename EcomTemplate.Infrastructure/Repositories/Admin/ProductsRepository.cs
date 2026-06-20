namespace EcomTemplate.Infrastructure.Services.Admin;

using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> DeleteProduct(Guid productId)
    {
        try
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if(product == null)
            {
                return false;
            }

            _db.Products.Remove(product);
            var affectedRows = await _db.SaveChangesAsync();

            return affectedRows > 0 ;
        }
        catch (Exception e)
        {
            
            Console.WriteLine($"Deteled database error: ", e);
            return false;
        }


    }

    public async Task<Product?> UpdateProduct(
Product updatedProduct,
Guid productId)
{
var product = await _db.Products
.Include(p => p.Images)
.Include(p => p.ProductVariants)
.FirstOrDefaultAsync(p => p.ProductId == productId);


if (product == null)
{
    return null;
}

// Product fields
product.Name = updatedProduct.Name;
product.Description = updatedProduct.Description;
product.CategoryId = updatedProduct.CategoryId;

// Replace Images
_db.ProductImages.RemoveRange(product.Images);

product.Images = [.. updatedProduct.Images
    .Select(i => new ProductImage
    {
        ImageUrl = i.ImageUrl,
        IsPrimary = i.IsPrimary
    })];

// Replace Variants
_db.ProductVariants.RemoveRange(product.ProductVariants);

product.ProductVariants = [.. updatedProduct.ProductVariants
    .Select(v => new ProductVariant
    {
        Sku = v.Sku,
        Price = v.Price,
        Stock = v.Stock,
        Attributes = v.Attributes
            .Select(a => new ProductVariantAttribute
            {
                Name = a.Name,
                Value = a.Value
            })
            .ToList()
    })];

await _db.SaveChangesAsync();

return product;


}

}