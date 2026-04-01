using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace GrocerySupermarket.Infrastructure.Repositories;
public class CustomerProfileRepository : ICustomerProfileRepository
{
    private readonly AppDbContext _db;

    public CustomerProfileRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CustomerProfile?> GetByIdAsync(Guid customerId)
    {
        return await _db.CustomerProfiles
            .FirstOrDefaultAsync(x => x.CustomerProfileId == customerId);
    }

    public async Task<CustomerProfile> CreateAsync(CustomerProfile profile)
    {
        _db.CustomerProfiles.Add(profile);
        await _db.SaveChangesAsync();
        return profile;
    }

    public async Task UpdateAsync(CustomerProfile profile)
    {
        _db.CustomerProfiles.Update(profile);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid customerId)
    {
        var profile = await GetByIdAsync(customerId);
        if (profile == null) return;

        _db.CustomerProfiles.Remove(profile);
        await _db.SaveChangesAsync();
    }

    public async Task<CustomerProfile?> GetByCustomerIdAsync(Guid customerId)
{
    return await _db.CustomerProfiles
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.CustomerProfileId == customerId);
}

}
