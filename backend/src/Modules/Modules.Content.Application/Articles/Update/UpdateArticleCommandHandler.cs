using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.Articles.Update;

internal sealed class UpdateArticleCommandHandler(
    IArticleRepository articleRepository,
    IContentRevisionRepository
        contentRevisionRepository,
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

        if (article.Version != request.ExpectedVersion)
        {
            throw new ConflictException(
                "Az írás időközben megváltozott. " +
                "A módosításaid nem kerültek mentésre. " +
                "Másold ki őket, mielőtt újratöltöd az írást.");
        }

        contentRevisionRepository.Add(
            Domain.ContentRevision.Capture(
                article));

        article.UpdateDetails(
            request.TitleHu,
            request.TitleEn,
            request.SummaryHu,
            request.SummaryEn,
            request.ContentHu,
            request.ContentEn);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}