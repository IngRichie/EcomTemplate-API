using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Application.Interfaces;

public interface IBannerRepository
{
    Task<List<Banner>> GetActiveAsync();
    Task<Banner?> GetByIdAsync(Guid id);

    Task AddAsync(Banner banner);
    Task UpdateAsync(Banner banner);
    Task DeleteAsync(Banner banner);

    Task SaveAsync();
}
