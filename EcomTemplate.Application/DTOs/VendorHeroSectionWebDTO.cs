using System;

namespace GrocerySupermarket.Application.DTOs;

public class VendorHeroSectionDTO
{
    public Guid Id { get; set; }

    public Guid VendorId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Subtitle { get; set; }

    public string MediaUrl { get; set; } = string.Empty;

    public string MediaType { get; set; } = "image";

    public string? CtaText { get; set; }

    public string? CtaUrl { get; set; }
}
