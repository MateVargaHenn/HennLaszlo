using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.GetAdminList;

internal sealed class GetAdminArticlesQueryHandler(
    IArticleRepository articleRepository)
    : IRequestHandler<
        GetAdminArticlesQuery,
        IReadOnlyList<AdminArticleListItem>>
{
    public async Task<
        IReadOnlyList<AdminArticleListItem>> Handle(
            GetAdminArticlesQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Article> articles =
            await articleRepository.GetAllAsync(
                cancellationToken);

        return articles
            .Select(
                article =>
                    new AdminArticleListItem(
                        article.Id,
                        article.Slug,
                        article.TitleHu,
                        article.TitleEn,
                        article.IsPublished,
                        article.DisplayOrder,
                        article.UpdatedAtUtc))
            .ToList();
    }
}