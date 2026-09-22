namespace Modules.Artwork.Application.Artworks.GetById;

public sealed record ArtworkDetails(
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
    bool IsFeatured);