using MediatR;
using Modules.Artwork.Application.Abstractions;
using Modules.FileStorage.Contracts;

namespace Modules.Artwork.Application.Artworks.GetImage;

internal sealed class GetArtworkImageQueryHandler(
    IArtworkRepository artworkRepository,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<
        GetArtworkImageQuery,
        ArtworkImage?>
{
    public async Task<ArtworkImage?> Handle(
        GetArtworkImageQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Artwork? artwork =
            request.IncludeUnpublished
                ? await artworkRepository.GetByIdAsync(
                    request.ArtworkId,
                    cancellationToken)
                : await artworkRepository.GetPublishedByIdAsync(
                    request.ArtworkId,
                    cancellationToken);

        if (artwork is null ||
            artwork.ImageId is null)
        {
            return null;
        }

        FileContentData? file =
            await fileStorageModule.GetFileContentAsync(
                artwork.ImageId.Value,
                cancellationToken);

        if (file is null ||
            !file.ContentType.StartsWith(
                "image/",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return new ArtworkImage(
            file.Content,
            file.ContentType);
    }
}