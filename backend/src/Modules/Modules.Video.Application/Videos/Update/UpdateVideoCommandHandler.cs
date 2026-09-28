using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Ordering;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.Update;

internal sealed class UpdateVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoUnitOfWork unitOfWork)
    : IRequestHandler<UpdateVideoCommand>
{
    public async Task Handle(
        UpdateVideoCommand request,
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

        video.UpdateDetails(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.DescriptionHu,
            request.DescriptionEn,
            request.VideoUrl);

        DisplayOrderManager.Place(
            orderedVideos,
            video,
            request.DisplayOrder,
            static (item, position) =>
                item.SetDisplayOrder(position));

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}