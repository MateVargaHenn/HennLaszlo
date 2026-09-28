using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Ordering;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.Update;

internal sealed class UpdateArtworkCommandHandler(
    IArtworkRepository artworkRepository,
    IArtworkUnitOfWork unitOfWork)
    : IRequestHandler<UpdateArtworkCommand>
{
    public async Task Handle(
        UpdateArtworkCommand request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Artwork>
            orderedArtworks =
                await artworkRepository
                    .GetOrderedForUpdateAsync(
                        cancellationToken);

        Domain.Artwork? artwork =
            orderedArtworks.SingleOrDefault(
                item =>
                    item.Id == request.ArtworkId);

        if (artwork is null)
        {
            throw new NotFoundException(
                "A mű nem található.");
        }

        if (request.IsFeatured &&
            !artwork.IsPublished)
        {
            throw new ValidationException(
                new[]
                {
                    new ValidationFailure(
                        nameof(request.IsFeatured),
                        "Csak publikált mű lehet kiemelt.")
                });
        }

        artwork.UpdateDetails(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.TechniqueHu,
            request.TechniqueEn,
            request.WidthCm,
            request.HeightCm,
            request.DescriptionHu,
            request.DescriptionEn);

        DisplayOrderManager.Place(
            orderedArtworks,
            artwork,
            request.DisplayOrder,
            static (item, position) =>
                item.SetDisplayOrder(position));
        
        if (request.IsFeatured)
        {
            IReadOnlyList<Domain.Artwork> featuredArtworks =
                await artworkRepository.GetFeaturedAsync(
                    cancellationToken);

            foreach (Domain.Artwork featuredArtwork
                    in featuredArtworks)
            {
                if (featuredArtwork.Id != artwork.Id)
                {
                    featuredArtwork.SetFeatured(false);
                }
            }
        }

        artwork.SetFeatured(
            request.IsFeatured);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}