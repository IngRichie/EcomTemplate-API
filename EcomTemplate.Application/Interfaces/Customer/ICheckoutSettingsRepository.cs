using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Infrastructure.Interfaces;
public interface ICheckoutSettingsRepository
{
    Task<CheckoutSettings> GetOrCreateSettings();
    Task UpdateSettings(CheckoutSettings settings);
}