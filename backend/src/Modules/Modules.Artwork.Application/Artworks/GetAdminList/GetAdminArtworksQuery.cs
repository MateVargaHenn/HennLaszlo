using MediatR;

namespace Modules.Artwork.Application.Artworks.GetAdminList;

public sealed record GetAdminArtworksQuery
    : IRequest<IReadOnlyList<AdminArtworkListItem>>;