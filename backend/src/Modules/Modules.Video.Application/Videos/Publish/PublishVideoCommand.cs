using MediatR;

namespace Modules.Video.Application.Videos.Publish;

public sealed record PublishVideoCommand(
    Guid VideoId)
    : IRequest;