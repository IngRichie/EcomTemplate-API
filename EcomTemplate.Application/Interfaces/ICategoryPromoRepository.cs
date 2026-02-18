using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface ICategoryPromoRepository
{
    Task<List<CategoryPromo>> GetActiveAsync();
    Task<CategoryPromo?> GetByIdAsync(Guid id);

    Task AddAsync(CategoryPromo promo);
    Task UpdateAsync(CategoryPromo promo);
    Task DeleteAsync(CategoryPromo promo);

    Task SaveAsync();
}
