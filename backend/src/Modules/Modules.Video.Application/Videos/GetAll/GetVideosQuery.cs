using MediatR;

namespace Modules.Video.Application.Videos.GetAll;

public sealed record GetVideosQuery
    : IRequest<IReadOnlyList<VideoListItem>>;