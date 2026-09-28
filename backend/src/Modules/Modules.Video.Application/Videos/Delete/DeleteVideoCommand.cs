using MediatR;

namespace Modules.Video.Application.Videos.Delete;

public sealed record DeleteVideoCommand(
    Guid VideoId)
    : IRequest;