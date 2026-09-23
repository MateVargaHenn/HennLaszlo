using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Update;

internal sealed class UpdateArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<UpdateArticleCommand>
{
    public async Task Handle(
        UpdateArticleCommand request,
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

        article.UpdateDetails(
            request.TitleHu,
            request.TitleEn,
            request.SummaryHu,
            request.SummaryEn,
            request.ContentHu,
            request.ContentEn,
            request.DisplayOrder);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}