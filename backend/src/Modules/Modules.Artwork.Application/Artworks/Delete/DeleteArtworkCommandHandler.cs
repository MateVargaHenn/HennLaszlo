using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Modules.Artwork.Application.Abstractions;
using Modules.FileStorage.Contracts;

namespace Modules.Artwork.Application.Artworks.Delete;

internal sealed class DeleteArtworkCommandHandler(
    IArtworkRepository artworkRepository,
    IArtworkUnitOfWork unitOfWork,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<DeleteArtworkCommand>
{
    public async Task Handle(
        DeleteArtworkCommand request,
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

        if (artwork.IsPublished)
        {
            throw new ValidationException(
                new[]
                {
                    new ValidationFailure(
                        nameof(request.ArtworkId),
                        "Publikált mű nem törölhető.")
                });
        }

		if (artwork.ImageId is Guid imageId)
		{
			await fileStorageModule.DeleteFileAsync(
				imageId,
				cancellationToken);
		}

        artworkRepository.Remove(artwork);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}