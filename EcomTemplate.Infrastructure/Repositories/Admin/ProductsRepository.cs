namespace EcomTemplate.Infrastructure.Services.Admin;

using EcomTemplate.Application.Interfaces.Admin;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;

public class AddProducts : IAddProducts
{
    private readonly AppDbContext _db;

    public AddProducts(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> AddNewProducts(List<Product> products)
    {
        await _db.AddRangeAsync(products);
        await _db.SaveChangesAsync();

        return products;
    }
}