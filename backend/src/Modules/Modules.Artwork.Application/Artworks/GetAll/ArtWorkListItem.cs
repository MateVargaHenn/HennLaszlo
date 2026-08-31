
namespace Modules.Artwork.Application.Artworks.GetAll;
public sealed record ArtworkListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    string? TechniqueEn,
    decimal? WidthCm,
    decimal? HeightCm,
    Guid? ImageId,
    bool IsFeatured,
    int DisplayOrder);
