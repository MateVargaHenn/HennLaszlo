using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Artwork.Application.Abstractions;

namespace Modules.Artwork.Application.Artworks.Publish;

internal sealed class PublishArtworkCommandHandler(
    IArtworkRepository artworkRepository,
    IArtworkUnitOfWork unitOfWork)
    : IRequestHandler<PublishArtworkCommand>
{
    public async Task Handle(
        PublishArtworkCommand request,
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

        artwork.Publish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}