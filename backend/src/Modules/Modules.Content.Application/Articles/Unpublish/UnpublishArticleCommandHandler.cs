using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Unpublish;

internal sealed class UnpublishArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<UnpublishArticleCommand>
{
    public async Task Handle(
        UnpublishArticleCommand request,
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

        article.Unpublish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}