using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.Publish;

internal sealed class PublishVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoUnitOfWork unitOfWork)
    : IRequestHandler<PublishVideoCommand>
{
    public async Task Handle(
        PublishVideoCommand request,
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

        video.Publish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}