using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.GetAll;

internal sealed class GetVideosQueryHandler(
    IVideoRepository videoRepository)
    : IRequestHandler<
        GetVideosQuery,
        IReadOnlyList<VideoListItem>>
{
    public async Task<IReadOnlyList<VideoListItem>> Handle(
        GetVideosQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Video> videos =
            await videoRepository.GetPublishedAsync(
                cancellationToken);

        return videos
            .Select(video => new VideoListItem(
                video.Id,
                video.TitleHu,
                video.TitleEn,
                video.Year,
                video.DescriptionHu,
                video.DescriptionEn,
                video.VideoUrl,
                video.DisplayOrder,
                video.CreatedAtUtc,
                video.UpdatedAtUtc ??
                video.CreatedAtUtc))
            .ToList();
    }
}