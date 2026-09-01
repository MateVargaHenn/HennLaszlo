namespace Modules.Artwork.Application.Artworks.GetAdminList;

public sealed record AdminArtworkListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    decimal? WidthCm,
    decimal? HeightCm,
    bool HasImage,
    bool IsPublished,
    bool IsFeatured,
    int DisplayOrder,
    DateTime CreatedAtUtc);