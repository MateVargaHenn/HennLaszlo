namespace Modules.Artwork.Presentation.Artworks.Update;

internal sealed record UpdateArtworkRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    string? TechniqueEn,
    decimal? WidthCm,
    decimal? HeightCm,
    string? DescriptionHu,
    string? DescriptionEn,
    bool IsFeatured,
    int DisplayOrder);