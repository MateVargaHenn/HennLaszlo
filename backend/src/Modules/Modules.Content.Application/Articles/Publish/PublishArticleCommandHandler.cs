using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Publish;

internal sealed class PublishArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<PublishArticleCommand>
{
    public async Task Handle(
        PublishArticleCommand request,
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

        article.Publish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}