using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Infrastructure.Interfaces;
public interface ICheckoutSettingsRepository
{
    Task<CheckoutSettings> GetOrCreateSettings();
    Task UpdateSettings(CheckoutSettings settings);
}