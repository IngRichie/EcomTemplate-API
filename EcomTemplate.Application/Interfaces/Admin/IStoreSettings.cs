using EcomTemplate.Application.DTOs.AdminDTOs;
using EcomTemplate.Domain.Entities;
using EcomTemplate.Domain.Entities.Admin;

namespace EcomTemplate.Application.Interfaces.Admin;

public interface IStoreSettings
{
    Task<StoreSettings?> GetStoreSettings();

    Task<StoreSettings> CreateStoreSettings(
        StoreSettingsDTO storeSettings
    );

    Task<StoreSettings> UpdateStoreSettings(
        StoreSettingsDTO storeSettings
    );
}