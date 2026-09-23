using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.GetPublishedBySlug;

internal sealed class GetPublishedArticleBySlugQueryHandler(
    IArticleRepository articleRepository)
    : IRequestHandler<
        GetPublishedArticleBySlugQuery,
        PublishedArticleDetails>
{
    public async Task<PublishedArticleDetails> Handle(
        GetPublishedArticleBySlugQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Article? article =
            await articleRepository
                .GetPublishedBySlugAsync(
                    request.Slug,
                    cancellationToken);

        if (article is null)
        {
            throw new NotFoundException(
                "Az írás nem található.");
        }

        return new PublishedArticleDetails(
            article.Id,
            article.Slug,
            article.TitleHu,
            article.TitleEn,
            article.SummaryHu,
            article.SummaryEn,
            article.ContentHu,
            article.ContentEn);
    }
}