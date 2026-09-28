using MediatR;

namespace Modules.Video.Application.Videos.GetAdminList;

public sealed record GetAdminVideosQuery
    : IRequest<IReadOnlyList<AdminVideoListItem>>;