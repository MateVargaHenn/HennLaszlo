using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.GetAdminList;

internal sealed class GetAdminArtworksQueryHandler(
    IArtworkRepository artworkRepository)
    : IRequestHandler<
        GetAdminArtworksQuery,
        IReadOnlyList<AdminArtworkListItem>>
{
    public async Task<IReadOnlyList<AdminArtworkListItem>> Handle(
        GetAdminArtworksQuery request,
        CancellationToken cancellationToken)
    {
        var artworks = await artworkRepository.GetAllAsync(
            cancellationToken);

        return artworks
            .Select(artwork => new AdminArtworkListItem(
                artwork.Id,
                artwork.TitleHu,
                artwork.TitleEn,
                artwork.Year,
                artwork.TechniqueHu,
                artwork.WidthCm,
                artwork.HeightCm,
                artwork.ImageId is not null,
                artwork.IsPublished,
                artwork.IsFeatured,
                artwork.DisplayOrder,
                artwork.CreatedAtUtc))
            .ToArray();
    }
}