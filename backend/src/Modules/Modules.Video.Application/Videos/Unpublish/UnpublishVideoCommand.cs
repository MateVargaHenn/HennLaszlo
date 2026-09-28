using MediatR;

namespace Modules.Video.Application.Videos.Unpublish;

public sealed record UnpublishVideoCommand(
    Guid VideoId)
    : IRequest;