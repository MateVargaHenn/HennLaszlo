using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.GetAdminById;

internal sealed class GetAdminArtworkByIdQueryHandler(
    IArtworkRepository artworkRepository)
    : IRequestHandler<
        GetAdminArtworkByIdQuery,
        AdminArtworkDetails>
{
    public async Task<AdminArtworkDetails> Handle(
        GetAdminArtworkByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Artwork? artwork =
            await artworkRepository.GetByIdAsync(
                request.ArtworkId,
                cancellationToken);

        if (artwork is null)
        {
            throw new NotFoundException(
                "A mű nem található.");
        }

        return new AdminArtworkDetails(
            artwork.Id,
            artwork.TitleHu,
            artwork.TitleEn,
            artwork.Year,
            artwork.TechniqueHu,
            artwork.TechniqueEn,
            artwork.WidthCm,
            artwork.HeightCm,
            artwork.DescriptionHu,
            artwork.DescriptionEn,
            artwork.ImageId.HasValue,
            artwork.IsPublished,
            artwork.IsFeatured,
            artwork.DisplayOrder);
    }
}