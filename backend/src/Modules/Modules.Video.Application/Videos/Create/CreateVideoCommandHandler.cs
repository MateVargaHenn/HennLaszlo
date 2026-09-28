using BuildingBlocks.Application.Ordering;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.Create;

internal sealed class CreateVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoUnitOfWork unitOfWork)
    : IRequestHandler<CreateVideoCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateVideoCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Video video =
            Domain.Video.Create(
                request.TitleHu,
                request.TitleEn,
                request.Year,
                request.DescriptionHu,
                request.DescriptionEn,
                request.VideoUrl,
                request.DisplayOrder);

        IReadOnlyList<Domain.Video> orderedVideos =
            await videoRepository
                .GetOrderedForUpdateAsync(
                    cancellationToken);

        DisplayOrderManager.Place(
            orderedVideos,
            video,
            request.DisplayOrder,
            static (item, position) =>
                item.SetDisplayOrder(position));

        videoRepository.Add(video);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return video.Id;
    }
}