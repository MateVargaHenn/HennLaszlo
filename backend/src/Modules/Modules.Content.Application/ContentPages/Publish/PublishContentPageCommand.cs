using MediatR;

namespace Modules.Content.Application.ContentPages.Publish;

public sealed record PublishContentPageCommand(
    string Key)
    : IRequest;