using System;

namespace GrocerySupermarket.Application.DTOs;

public class VendorMobileHeroDTO
{
    public Guid Id { get; set; }

    public Guid VendorId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public string? TargetUrl { get; set; }
}
