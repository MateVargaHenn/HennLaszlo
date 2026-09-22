namespace Modules.Artwork.Application.Artworks.GetAdminById;

public sealed record AdminArtworkDetails(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    string? TechniqueEn,
    decimal? WidthCm,
    decimal? HeightCm,
    string? DescriptionHu,
    string? DescriptionEn,
    bool HasImage,
    bool IsPublished,
    bool IsFeatured,
    int DisplayOrder);