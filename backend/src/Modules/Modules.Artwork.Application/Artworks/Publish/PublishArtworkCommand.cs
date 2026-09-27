using MediatR;

namespace Modules.Artwork.Application.Artworks.Publish;

public sealed record PublishArtworkCommand(
    Guid ArtworkId)
    : IRequest;