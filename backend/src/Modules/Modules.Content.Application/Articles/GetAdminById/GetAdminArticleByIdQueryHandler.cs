using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.GetAdminById;

internal sealed class GetAdminArticleByIdQueryHandler(
    IArticleRepository articleRepository)
    : IRequestHandler<
        GetAdminArticleByIdQuery,
        AdminArticleDetails>
{
    public async Task<AdminArticleDetails> Handle(
        GetAdminArticleByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Article? article =
            await articleRepository.GetByIdAsync(
                request.ArticleId,
                cancellationToken);

        if (article is null)
        {
            throw new NotFoundException(
                "Az írás nem található.");
        }

        return new AdminArticleDetails(
            article.Id,
            article.Slug,
            article.TitleHu,
            article.TitleEn,
            article.SummaryHu,
            article.SummaryEn,
            article.ContentHu,
            article.ContentEn,
            article.IsPublished,
            article.DisplayOrder,
            article.CreatedAtUtc,
            article.UpdatedAtUtc);
    }
}