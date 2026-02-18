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
    return await _dbContext.Categories
        .OrderBy(c => c.Name)
        .Take(categoryLimit)
        .Select(c => new CategoryDTO
        {
            Id = c.CategoryId,
            Name = c.Name,
            // Slug = c.Name.ToLower().Replace(" ", "-"),

            Products = c.Products
                .OrderBy(p => p.Name)
                .Take(productsPerCategory)
                .Select(p => new ProductSummaryDTO
                {
                    Id = p.ProductId,
                    Name = p.Name,

                    PrimaryImage = p.Images   
                        .Where(i => i.IsPrimary)
                        .Select(i => new ProductImageDTO
                        {
                            ImageId = i.ProductImageId,
                            Url = i.ImageUrl,
                            IsPrimary = i.IsPrimary
                        })
                        .FirstOrDefault()
                })
                .ToList()
        })
        .ToListAsync();
}


}
