namespace Modules.Artwork.Presentation.Artworks.Create;

public sealed record CreateArtworkRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    string? TechniqueEn,
    decimal? WidthCm,
    decimal? HeightCm);