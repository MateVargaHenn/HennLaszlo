using MediatR;

namespace Modules.Artwork.Application.Artworks.GetAdminById;

public sealed record GetAdminArtworkByIdQuery(
    Guid ArtworkId
) : IRequest<AdminArtworkDetails>;