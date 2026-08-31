using MediatR;
using Modules.Artwork.Application.Abstractions;

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
            request.HeightCm);

        artworkRepository.Add(artwork);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return artwork.Id;
    }
}
