using MediatR;

namespace Modules.Content.Application.Articles.Delete;

public sealed record DeleteArticleCommand(
    Guid ArticleId)
    : IRequest;