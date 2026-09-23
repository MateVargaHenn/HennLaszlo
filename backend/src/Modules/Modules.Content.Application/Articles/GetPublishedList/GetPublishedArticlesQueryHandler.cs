using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.GetPublishedList;

internal sealed class GetPublishedArticlesQueryHandler(
    IArticleRepository articleRepository)
    : IRequestHandler<
        GetPublishedArticlesQuery,
        IReadOnlyList<PublishedArticleListItem>>
{
    public async Task<
        IReadOnlyList<PublishedArticleListItem>> Handle(
            GetPublishedArticlesQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Article> articles =
            await articleRepository.GetPublishedAsync(
                cancellationToken);

        return articles
            .Select(
                article =>
                    new PublishedArticleListItem(
                        article.Id,
                        article.Slug,
                        article.TitleHu,
                        article.TitleEn,
                        article.SummaryHu,
                        article.SummaryEn))
            .ToList();
    }
}