using MediatR;

namespace Modules.Artwork.Application.Artworks.AttachImage;

public sealed record AttachArtworkImageCommand(
    Guid ArtworkId,
    Guid FileId) : IRequest;