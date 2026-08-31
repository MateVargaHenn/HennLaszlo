using FluentValidation;
using FluentValidation.Results;
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
    public async Task Handle(
        AttachArtworkImageCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Artwork? artwork =
            await artworkRepository.GetByIdAsync(
                request.ArtworkId,
                cancellationToken);

        if (artwork is null)
        {
            throw new KeyNotFoundException(
                "A mű nem található.");
        }

        FileMetadata? file =
            await fileStorageModule.GetFileMetadataAsync(
                request.FileId,
                cancellationToken);

        if (file is null)
        {
            throw new KeyNotFoundException(
                "A fájl nem található.");
        }

        if (!file.ContentType.StartsWith(
                "image/",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException(
            [
                new ValidationFailure(
                    nameof(request.FileId),
                    "A műhöz csak képfájl kapcsolható.")
            ]);
        }

        artwork.SetImage(request.FileId);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}