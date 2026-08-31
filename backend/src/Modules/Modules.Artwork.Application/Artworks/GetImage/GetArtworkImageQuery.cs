using MediatR;

namespace Modules.Artwork.Application.Artworks.GetImage;

public sealed record GetArtworkImageQuery(
    Guid ArtworkId) : IRequest<ArtworkImage?>;