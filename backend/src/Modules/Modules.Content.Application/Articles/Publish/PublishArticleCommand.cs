using MediatR;

namespace Modules.Content.Application.Articles.Publish;

public sealed record PublishArticleCommand(
    Guid ArticleId)
    : IRequest;