using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using GrocerySupermarket.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

 public async Task<ProductDTO?> GetProductDetails(Guid productId)
{
    var product = await _dbContext.Products
        .Where(p => p.ProductId == productId)
        .Include(p => p.ProductVariants)
        .ThenInclude(v => v.Attributes)
        .Include(p => p.Reviews)
        .ThenInclude(r => r.CustomerProfile)
        .Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
     

            CategoryId = p.Category.CategoryId,
            CategoryName = p.Category.Name,


            Images = p.Images.Select(i => new ProductImageDTO
            {
                ImageId = i.ProductImageId,
                Url = i.ImageUrl,
                IsPrimary = i.IsPrimary
            }).ToList(),

            ProductVariants = p.ProductVariants.Select(pv => new ProductVariantDTO {
                ProductVariantId = pv.ProductVariantId,
                Sku = pv.Sku,
                Price = pv.Price,
                Stock = pv.Stock,

                Attributes = pv.Attributes.Select(a => new ProductVariantAttributeDTO
    {
        ProductVariantAttributeId = a.ProductVariantAttributeId,
        Name = a.Name,
        Value = a.Value
    }).ToList()
              
            }).ToList(),
            Reviews = p.Reviews.Select(r => new ProductReviewDTO
            {
                ProductReviewId = r.ProductReviewId,
                ProductId = r.ProductId,
                CustomerProfileId = r.CustomerProfileId,
                Rating = r.Rating,
                Comment = r.Comment,

                ReviewerInfo = new CustomerReviewDTO
                {
                    CustomerId = r.CustomerProfile.CustomerProfileId,
                    FirstName = r.CustomerProfile.FirstName
                }
            }).ToList()
        })
        .FirstOrDefaultAsync();

    return product;
}



    // public async Task<List<Category>> GetProductsByCategory(
    //     int page,
    //     int pageSize)
    // {
    //     if (page <= 0) page = 1;
    //     if (pageSize <= 0) pageSize = 10;

    //     int skip = (page - 1) * pageSize;

    //     // var products = await _dbContext.Products
    //     //     .Include(x => x.Category)
    //     //     .Where(x => x.Category != null && x.Category.Name == categoryName)
    //     //     .OrderBy(x => x.ProductId) // required for pagination stability
    //     //     .Skip(skip)
    //     //     .Take(pageSize)
    //     //     .ToListAsync();

    //     // return products;

    //     var productCategories = await _dbContext.Categories
    //                             .Include(x => x.Products)
    //                             .ThenInclude(x => x.Images)
    //                             .OrderByDescending(x => x.CreatedAt)
    //                             .Skip(skip)
    //                             .Take(pageSize)
    //                             .ToListAsync();

    //     return productCategories;



    // }

    public async Task<Category?> GetACategoryWithProducts(Guid categoryId)
    {
        var category = await _dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        return category;
    }


public async Task<List<Product>> GetTopProducts(int limit)
{
    if (limit <= 0) limit = 10;

    var productIds = await _dbContext.ProductReviews
        .GroupBy(r => r.ProductId)
        .OrderByDescending(g => g.Count())
        .Select(g => g.Key)
        .Take(limit)
        .ToListAsync();

    return await _dbContext.Products
        .Where(p => productIds.Contains(p.ProductId))
        .Include(p => p.Images)
        .Include(p => p.Reviews)
        .Include(p => p.Category)
        
        .ToListAsync();
}




public async Task<List<Product>> GetNewProducts(int limit)
{
    if (limit <= 0) limit = 10;

    return await _dbContext.Products
        .AsNoTracking()
        .OrderByDescending(p => p.CreatedAt)
        .Take(limit)
        .Select(p => new Product
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            CreatedAt = p.CreatedAt,

         

            Category = new Category
            {
                CategoryId = p.Category.CategoryId,
                Name = p.Category.Name
            },

            Images = p.Images
                .Where(i => i.IsPrimary)
                .Select(i => new ProductImage
                {
                    ProductImageId = i.ProductImageId,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary
                })
                .Take(1)
                .ToList(),

            ProductVariants = p.ProductVariants
                .OrderBy(v => v.CreatedAt)
                .Select(v => new ProductVariant
                {
                    ProductVariantId = v.ProductVariantId,
                    Price = v.Price,
                    Sku = v.Sku,
                    Stock = v.Stock
                })
                .Take(1)
                .ToList()
        })
        .ToListAsync();
}




public async Task<List<Product>> GetMostPopularProductsAsync(int limit)
{
    var productIds = await _dbContext.OrderItems
        .GroupBy(oi => oi.ProductId)
        .OrderByDescending(g => g.Sum(x => x.Quantity))
        .Select(g => g.Key)
        .Take(limit)
        .ToListAsync();

    return await _dbContext.Products
        .Where(p => productIds.Contains(p.ProductId))
        .Include(p => p.Images)
        .Include(p => p.Category)
     
        .Include(p => p.Reviews)
        .ToListAsync();
}

}
