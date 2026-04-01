using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GrocerySupermarket.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // public async Task<List<Category>> GetCategoriesWithProductsAsync(
    //     int categoryLimit,
    //     int productsPerCategory)
    // {
    //     // 1️⃣ Load categories first (LIMITED)
    //     var categories = await _dbContext.Categories
    //         .OrderBy(c => c.Name)
    //         .Take(categoryLimit)
    //         .ToListAsync();

    //     if (!categories.Any())
    //         return categories;

    //     var categoryIds = categories.Select(c => c.CategoryId).ToList();

    //     // 2️⃣ Load products separately (EF-safe)
    //     var products = await _dbContext.Products
    //         .Where(p => categoryIds.Contains(p.CategoryId))
    //         .Include(p => p.Images)
    //         .OrderBy(p => p.ProductId)
    //         .ToListAsync();

    //     // 3️⃣ Attach limited products to each category
    //     foreach (var category in categories)
    //     {
    //         category.Products = products
    //             .Where(p => p.CategoryId == category.CategoryId)
    //             .Take(productsPerCategory)
    //             .ToList();
    //     }

    //     return categories;
    // }


public async Task<List<CategoryDTO>> GetAllCategoriesAsync(
    int categoryLimit,
    int productsPerCategory)
{
    var categories = await _dbContext.Categories
    .Include(c => c.Products)
        .ThenInclude(p => p.Images)
    .Include(c => c.Products)
        .ThenInclude(p => p.ProductVariants)
    .OrderBy(c => c.Name)
    .Take(categoryLimit)
    .ToListAsync();

var dto = categories.Select(c => new CategoryDTO
{
    Id = c.CategoryId,
    Name = c.Name,
    Products = c.Products
        .OrderBy(p => p.Name)
        .Take(productsPerCategory)
        .Select(p => new ProductSummaryDTO
        {
            Id = p.ProductId,
            Name = p.Name,
            Description = p.Description ?? "No description available",
            PrimaryImage = p.Images.FirstOrDefault(i => i.IsPrimary) is { } img
                ? new ProductImageDTO
                {
                    ImageId = img.ProductImageId,
                    Url = img.ImageUrl,
                    IsPrimary = img.IsPrimary
                }
                : null,
            Variant = p.ProductVariants.OrderBy(v => v.Price).FirstOrDefault() is { } v
                ? new ProductVariantDTO
                {
                    ProductVariantId = v.ProductVariantId,
                    Sku = v.Sku,
                    Price = v.Price,
                    Stock = v.Stock
                }
                : null
        })
        .ToList()
}).ToList();

return dto;
}


}
