using MediatR;

namespace Modules.Content.Application.Articles.Unpublish;

public sealed record UnpublishArticleCommand(
    Guid ArticleId)
    : IRequest;