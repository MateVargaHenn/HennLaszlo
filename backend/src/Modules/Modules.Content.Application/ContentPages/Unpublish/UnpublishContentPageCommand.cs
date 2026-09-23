using MediatR;

namespace Modules.Content.Application.ContentPages.Unpublish;

public sealed record UnpublishContentPageCommand(
    string Key)
    : IRequest;