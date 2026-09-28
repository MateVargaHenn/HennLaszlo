using MediatR;
using Modules.Artwork.Application.Abstractions;
using BuildingBlocks.Application.Ordering;

namespace Modules.Artwork.Application.Artworks.Create;

internal sealed class CreateArtworkCommandHandler(
    IArtworkRepository artworkRepository,
    IArtworkUnitOfWork unitOfWork)
    : IRequestHandler<CreateArtworkCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateArtworkCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Artwork artwork = Domain.Artwork.Create(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.TechniqueHu,
            request.TechniqueEn,
            request.WidthCm,
            request.HeightCm,
            request.DescriptionHu,
            request.DescriptionEn,
            request.IsFeatured,
            request.DisplayOrder);

        IReadOnlyList<Domain.Artwork>
            orderedArtworks =
                await artworkRepository
                    .GetOrderedForUpdateAsync(
                        cancellationToken);

        DisplayOrderManager.Place(
            orderedArtworks,
            artwork,
            request.DisplayOrder,
            static (item, position) =>
                item.SetDisplayOrder(position));

        artworkRepository.Add(artwork);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return artwork.Id;
    }
}
