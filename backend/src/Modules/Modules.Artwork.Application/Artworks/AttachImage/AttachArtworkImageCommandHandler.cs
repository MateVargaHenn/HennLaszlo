using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Artwork.Application.Abstractions;
using Modules.FileStorage.Contracts;

namespace Modules.Artwork.Application.Artworks.AttachImage;

internal sealed class AttachArtworkImageCommandHandler(
IArtworkRepository artworkRepository,
IArtworkUnitOfWork unitOfWork,
IFileStorageModule fileStorageModule)
: IRequestHandler<AttachArtworkImageCommand>
{
    public async Task Handle(AttachArtworkImageCommand request, CancellationToken cancellationToken)
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

		FileMetadata? file =
			await fileStorageModule.GetFileMetadataAsync(
				request.FileId,
				cancellationToken);

		if (file is null)
		{
			throw new NotFoundException(
				"A feltöltött fájl nem található.");
		}

		if (!file.ContentType.StartsWith(
				"image/",
				StringComparison.OrdinalIgnoreCase))
		{
			throw new ValidationException(
				"A műhöz kizárólag kép csatolható.");
		}

		if (artwork.ImageId == request.FileId)
		{
			return;
		}

		Guid? previousImageId = artwork.ImageId;

		artwork.SetImage(request.FileId);

		await unitOfWork.SaveChangesAsync(
			cancellationToken);

		if (previousImageId.HasValue)
		{
			await fileStorageModule.DeleteFileAsync(
				previousImageId.Value,
				cancellationToken);
		}
	}
}
