using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.Unpublish;

internal sealed class UnpublishArtworkCommandHandler(
    IArtworkRepository artworkRepository,
    IArtworkUnitOfWork unitOfWork)
    : IRequestHandler<UnpublishArtworkCommand>
{
    public async Task Handle(
        UnpublishArtworkCommand request,
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

        artwork.Unpublish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}