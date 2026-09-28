using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Ordering;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.Delete;

internal sealed class DeleteVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoUnitOfWork unitOfWork)
    : IRequestHandler<DeleteVideoCommand>
{
    public async Task Handle(
        DeleteVideoCommand request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Video> orderedVideos =
            await videoRepository
                .GetOrderedForUpdateAsync(
                    cancellationToken);

        Domain.Video? video =
            orderedVideos.SingleOrDefault(item =>
                item.Id == request.VideoId);

        if (video is null)
        {
            throw new NotFoundException(
                "A videó nem található.");
        }

        if (video.IsPublished)
        {
            throw new InvalidOperationException(
                "Publikált videó nem törölhető.");
        }

        videoRepository.Remove(video);

        DisplayOrderManager.Remove(
            orderedVideos,
            video,
            static (item, position) =>
                item.SetDisplayOrder(position));

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}