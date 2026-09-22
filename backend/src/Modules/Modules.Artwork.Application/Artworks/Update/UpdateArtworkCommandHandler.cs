using BuildingBlocks.Application.Exceptions;
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
        Domain.Artwork? artwork =
            await artworkRepository.GetByIdAsync(
                request.ArtworkId,
                cancellationToken);

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
            request.DescriptionEn,
            request.DisplayOrder);

        artwork.SetDisplayOrder(
            request.DisplayOrder);

        artwork.SetFeatured(
            request.IsFeatured);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}