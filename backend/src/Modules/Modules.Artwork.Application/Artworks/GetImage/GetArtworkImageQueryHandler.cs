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
            await artworkRepository.GetPublishedByIdAsync(
                request.ArtworkId,
                cancellationToken);

        if (artwork?.ImageId is not Guid imageId)
        {
            return null;
        }

        FileContentData? file =
            await fileStorageModule.GetFileContentAsync(
                imageId,
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