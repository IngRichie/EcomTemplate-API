using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly AppDbContext _db;

    public BannerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Banner>> GetActiveAsync()
    {
        return await _db.Banners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Banner?> GetByIdAsync(Guid id)
    {
        return await _db.Banners.FindAsync(id);
    }

    public async Task AddAsync(Banner banner)
    {
        await _db.Banners.AddAsync(banner);
    }

    public Task UpdateAsync(Banner banner)
    {
        _db.Banners.Update(banner);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Banner banner)
    {
        _db.Banners.Remove(banner);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
