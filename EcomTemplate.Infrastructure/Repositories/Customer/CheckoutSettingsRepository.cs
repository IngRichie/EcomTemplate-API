using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Infrastructure.Data;
using EcomTemplate.Infrastructure.Interfaces;
using EcomTemplate.Infrastructure.Options;

namespace EcomTemplate.Infrastructure.Repositories;



public class CheckoutSettingsRepository : ICheckoutSettingsRepository
{
    private readonly AppDbContext _db;
    private readonly CheckoutDefaultsOptions _defaults;

    public CheckoutSettingsRepository(
        AppDbContext db,
        IOptions<CheckoutDefaultsOptions> defaults)
    {
        _db = db;
        _defaults = defaults.Value;
    }

    public async Task<CheckoutSettings> GetOrCreateSettings()
    {
        var settings = await _db.CheckoutSettings.FirstOrDefaultAsync();

        // ✅ EXPLICIT fallback (not magic)
        if (settings == null)
        {
            settings = new CheckoutSettings
            {
                Id = Guid.NewGuid(),
                TaxPercentage = _defaults.TaxPercentage,
                DeliveryFee = _defaults.DeliveryFee,
                ReferralDiscountPercentage = _defaults.ReferralDiscountPercentage
            };

            _db.CheckoutSettings.Add(settings);
            await _db.SaveChangesAsync();
        }

        return settings;
    }

    public async Task UpdateSettings(CheckoutSettings settings)
    {
        var existing = await GetOrCreateSettings();

        existing.TaxPercentage = settings.TaxPercentage;
        existing.DeliveryFee = settings.DeliveryFee;
        existing.ReferralDiscountPercentage = settings.ReferralDiscountPercentage;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}
