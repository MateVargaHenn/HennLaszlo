using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.Unpublish;

internal sealed class UnpublishVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoUnitOfWork unitOfWork)
    : IRequestHandler<UnpublishVideoCommand>
{
    public async Task Handle(
        UnpublishVideoCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Video? video =
            await videoRepository.GetByIdAsync(
                request.VideoId,
                cancellationToken);

        if (video is null)
        {
            throw new NotFoundException(
                "A videó nem található.");
        }

        video.Unpublish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}