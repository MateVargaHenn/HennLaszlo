using MediatR;

namespace Modules.Content.Application.Articles.GetPublishedList;

public sealed record GetPublishedArticlesQuery
    : IRequest<IReadOnlyList<PublishedArticleListItem>>;