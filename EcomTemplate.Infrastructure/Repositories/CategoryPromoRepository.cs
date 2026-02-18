using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class CategoryPromoRepository : ICategoryPromoRepository
{
    private readonly AppDbContext _db;

    public CategoryPromoRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoryPromo>> GetActiveAsync()
    {
        return await _db.CategoryPromos
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();
    }

    public async Task<CategoryPromo?> GetByIdAsync(Guid id)
    {
        return await _db.CategoryPromos.FindAsync(id);
    }

    public async Task AddAsync(CategoryPromo promo)
    {
        await _db.CategoryPromos.AddAsync(promo);
    }

    public Task UpdateAsync(CategoryPromo promo)
    {
        _db.CategoryPromos.Update(promo);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CategoryPromo promo)
    {
        _db.CategoryPromos.Remove(promo);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
