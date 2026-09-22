using MediatR;

namespace Modules.Artwork.Application.Artworks.Delete;

public sealed record DeleteArtworkCommand(
    Guid ArtworkId
) : IRequest;