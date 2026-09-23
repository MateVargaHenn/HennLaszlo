using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Delete;

internal sealed class DeleteArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<DeleteArticleCommand>
{
    public async Task Handle(
        DeleteArticleCommand request,
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

        if (article.IsPublished)
        {
            throw new InvalidOperationException(
                "Publikált írás nem törölhető.");
        }

        articleRepository.Remove(article);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}