using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces;

public interface ICategoryRepository
{
    // Task<List<Category>> GetCategoriesWithProductsAsync(
    //     int categoryLimit,
    //     int productsPerCategory);

    Task<List<CategoryDTO>> GetAllCategoriesAsync(
    int categoryLimit,
    int productsPerCategory);

    

}
