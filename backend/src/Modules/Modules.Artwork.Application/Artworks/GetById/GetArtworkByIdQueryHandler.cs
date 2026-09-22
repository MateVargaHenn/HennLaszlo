using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.GetById;

internal sealed class GetArtworkByIdQueryHandler(
    IArtworkRepository artworkRepository)
    : IRequestHandler<GetArtworkByIdQuery, ArtworkDetails>
{
    public async Task<ArtworkDetails> Handle(
        GetArtworkByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Artwork? artwork =
            await artworkRepository.GetByIdAsync(
                request.ArtworkId,
                cancellationToken);

        if (artwork is null || !artwork.IsPublished)
        {
            throw new NotFoundException(
                "A mű nem található.");
        }

        return new ArtworkDetails(
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
            artwork.IsFeatured);
    }
}