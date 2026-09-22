using MediatR;

namespace Modules.Artwork.Application.Artworks.GetAll;

public sealed record GetArtworksQuery
    : IRequest<IReadOnlyCollection<ArtworkListItem>>;