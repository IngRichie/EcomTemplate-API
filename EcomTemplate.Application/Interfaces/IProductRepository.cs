using System.Collections.Generic;
using System.Threading.Tasks;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Application.DTOs;
namespace GrocerySupermarket.Application.Interfaces;

public interface IProductRepository
{
    // EVERYTHING WHERE WILL BE LOADED TO REDIS FOR CACHING.
    // Task<List<Category>> GetAllCategories();
    Task<Category?> GetACategoryWithProducts(Guid categoryId);
    // Task<List<Category>> GetProductsByCategory(int page,
    //     int pageSize);
    Task<ProductDTO?> GetProductDetails(Guid productId);

    Task<List<Product>> GetTopProducts(int limit);

Task<List<Product>> GetNewProducts(int limit);
Task<List<Product>> GetMostPopularProductsAsync(int limit);

}