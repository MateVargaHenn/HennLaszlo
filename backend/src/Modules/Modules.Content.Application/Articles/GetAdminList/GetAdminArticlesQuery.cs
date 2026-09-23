using MediatR;

namespace Modules.Content.Application.Articles.GetAdminList;

public sealed record GetAdminArticlesQuery
    : IRequest<IReadOnlyList<AdminArticleListItem>>;