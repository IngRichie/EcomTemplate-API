using System;

namespace GrocerySupermarket.Application.DTOs;

public class VideoAdDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string VideoUrl { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public string? TargetUrl { get; set; }

    public bool IsSkippable { get; set; }

    public int SkipAfterSeconds { get; set; }

    public int DisplayOrder { get; set; }
}
