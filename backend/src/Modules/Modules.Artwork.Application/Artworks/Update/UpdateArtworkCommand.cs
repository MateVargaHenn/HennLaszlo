using MediatR;
using Modules.Artwork.Application.Artworks.Common;

namespace Modules.Artwork.Application.Artworks.Update;

public sealed record UpdateArtworkCommand(
    Guid ArtworkId,
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
    int DisplayOrder
) : IRequest, IArtworkDetailsCommand;