using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.GetAdminList;

internal sealed class GetAdminVideosQueryHandler(
    IVideoRepository videoRepository)
    : IRequestHandler<
        GetAdminVideosQuery,
        IReadOnlyList<AdminVideoListItem>>
{
    public async Task<
        IReadOnlyList<AdminVideoListItem>> Handle(
            GetAdminVideosQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Video> videos =
            await videoRepository.GetAllAsync(
                cancellationToken);

        return videos
            .Select(video =>
                new AdminVideoListItem(
                    video.Id,
                    video.TitleHu,
                    video.TitleEn,
                    video.Year,
                    video.VideoUrl,
                    video.IsPublished,
                    video.DisplayOrder,
                    video.CreatedAtUtc))
            .ToList();
    }
}