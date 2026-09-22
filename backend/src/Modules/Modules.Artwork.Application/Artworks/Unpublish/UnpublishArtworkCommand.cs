using MediatR;

namespace Modules.Artwork.Application.Artworks.Unpublish;

public sealed record UnpublishArtworkCommand(
    Guid ArtworkId)
    : IRequest;