using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.GetAll;

internal class GetArtworksQueryHandler(
    IArtworkRepository artworkRepository)
    : IRequestHandler<
        GetArtworksQuery,
        IReadOnlyCollection<ArtworkListItem>>
{
    public async Task<IReadOnlyCollection<ArtworkListItem>> Handle(
        GetArtworksQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Domain.Artwork> artworks =
            await artworkRepository.GetPublishedAsync(cancellationToken);

        return artworks
            .Select(artwork => new ArtworkListItem(
                artwork.Id,
                artwork.TitleHu,
                artwork.TitleEn,
                artwork.Year,
                artwork.TechniqueHu,
                artwork.TechniqueEn,
                artwork.WidthCm,
                artwork.HeightCm,
                artwork.ImageId,
                artwork.IsFeatured,
                artwork.DisplayOrder))
            .ToArray();
    }
}