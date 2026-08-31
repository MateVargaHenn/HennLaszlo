using MediatR;

namespace Modules.Artwork.Application.Artworks.Create;

public sealed record CreateArtworkCommand(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? TechniqueHu,
    string? TechniqueEn,
    decimal? WidthCm,
    decimal? HeightCm) : IRequest<Guid>;
