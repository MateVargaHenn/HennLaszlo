using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Application.Videos.GetAdminById;

internal sealed class GetAdminVideoByIdQueryHandler(
    IVideoRepository videoRepository)
    : IRequestHandler<
        GetAdminVideoByIdQuery,
        AdminVideoDetails>
{
    public async Task<AdminVideoDetails> Handle(
        GetAdminVideoByIdQuery request,
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

        return new AdminVideoDetails(
            video.Id,
            video.TitleHu,
            video.TitleEn,
            video.Year,
            video.DescriptionHu,
            video.DescriptionEn,
            video.VideoUrl,
            video.IsPublished,
            video.DisplayOrder);
    }
}