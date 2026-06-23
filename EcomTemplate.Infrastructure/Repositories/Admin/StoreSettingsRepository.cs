namespace EcomTemplate.Infrastructure.Services.Admin;

using System.Threading.Tasks;
using AutoMapper;
using EcomTemplate.Application.DTOs.AdminDTOs;
using EcomTemplate.Application.Interfaces.Admin;

using EcomTemplate.Domain.Entities.Admin;
using EcomTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class StoreSettingsRepository : IStoreSettings
{
    private readonly AppDbContext _db;
    private readonly Mapper _mapper;

    public StoreSettingsRepository(AppDbContext db, Mapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<StoreSettings> CreateStoreSettings(
    StoreSettingsDTO storeSettings
)
{
    var existing =
        await _db.StoreSettings
            .FirstOrDefaultAsync();

    if (existing != null)
    {
        throw new Exception(
            "Store settings already exist"
        );
    }

    var settings =
        _mapper.Map<StoreSettings>(
            storeSettings
        );

    await _db.StoreSettings.AddAsync(
        settings
    );

    await _db.SaveChangesAsync();

    return settings;
}

    public async Task<StoreSettings> UpdateStoreSettings(
    StoreSettingsDTO dto
)
{
    var settings =
        await _db.StoreSettings
            .FirstOrDefaultAsync();

    if (settings == null)
    {
        throw new Exception(
            "Store settings not found"
        );
    }

    settings.StoreName = dto.StoreName;
    settings.StoreEmail = dto.StoreEmail;
    settings.StorePhone = dto.StorePhone;
    settings.StoreAddress = dto.StoreAddress;

    settings.Currency = dto.Currency;

    settings.DeliveryFee =
        dto.DeliveryFee;

    settings.FreeDeliveryThreshold =
        dto.FreeDeliveryThreshold;

    settings.EstimatedDeliveryTime =
        dto.EstimatedDeliveryTime;

    settings.TaxPercentage =
        dto.TaxPercentage;

    settings.SupportWhatsApp =
        dto.SupportWhatsApp;

    settings.SupportEmail =
        dto.SupportEmail;

    settings.UpdatedAt =
        DateTime.UtcNow;

    await _db.SaveChangesAsync();

    return settings;
}

public async Task<StoreSettings?> GetStoreSettings()
{
    return await _db.StoreSettings
        .FirstOrDefaultAsync();
}
}