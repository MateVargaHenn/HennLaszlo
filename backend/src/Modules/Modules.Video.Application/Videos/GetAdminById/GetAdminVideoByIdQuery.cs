using MediatR;

namespace Modules.Video.Application.Videos.GetAdminById;

public sealed record GetAdminVideoByIdQuery(
    Guid VideoId)
    : IRequest<AdminVideoDetails>;