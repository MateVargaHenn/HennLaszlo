using MediatR;

namespace Modules.Artwork.Application.Artworks.GetById;

public sealed record GetArtworkByIdQuery(
    Guid ArtworkId)
    : IRequest<ArtworkDetails>;