using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application
    .ContentRevisions.Restore;

internal sealed class
    RestoreContentRevisionCommandHandler(
        IContentRevisionRepository
            contentRevisionRepository,
        IArticleRepository articleRepository,
        IContentPageRepository
            contentPageRepository,
        IContentUnitOfWork unitOfWork)
    : IRequestHandler<
        RestoreContentRevisionCommand>
{
    public async Task Handle(
        RestoreContentRevisionCommand request,
        CancellationToken cancellationToken)
    {
        Domain.ContentRevision? revision =
            await contentRevisionRepository
                .GetByIdAsync(
                    request.RevisionId,
                    request.TargetType,
                    request.TargetId,
                    cancellationToken);

        if (revision is null)
        {
            throw new NotFoundException(
                "A korábbi tartalomverzió " +
                "nem található.");
        }

        switch (request.TargetType)
        {
            case Domain
                .ContentRevisionTargetType
                .Article:
                await RestoreArticleAsync(
                    request.TargetId,
                    revision,
                    request.ExpectedVersion,
                    cancellationToken);
                break;

            case Domain
                .ContentRevisionTargetType
                .ContentPage:
                await RestoreContentPageAsync(
                    request.TargetId,
                    revision,
                    request.ExpectedVersion,
                    cancellationToken);
                break;

            default:
                throw new InvalidOperationException(
                    "A tartalomtípus nem támogatott.");
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    private async Task RestoreArticleAsync(
        Guid articleId,
        Domain.ContentRevision revision,
        Guid expectedVersion,
        CancellationToken cancellationToken)
    {
        Domain.Article? article =
            await articleRepository.GetByIdAsync(
                articleId,
                cancellationToken);

        if (article is null)
        {
            throw new NotFoundException(
                "Az írás nem található.");
        }

        if (expectedVersion == Guid.Empty ||
            article.Version != expectedVersion)
        {
            throw new ConflictException(
                "A tartalom időközben megváltozott. " +
                "A visszaállítás nem történt meg. " +
                "Töltsd újra az oldalt, és ellenőrizd " +
                "az aktuális tartalmat.");
        }

        contentRevisionRepository.Add(
            Domain.ContentRevision.Capture(
                article));

        article.UpdateDetails(
            revision.TitleHu,
            revision.TitleEn,
            revision.SummaryHu,
            revision.SummaryEn,
            revision.ContentHu,
            revision.ContentEn);
    }

    private async Task RestoreContentPageAsync(
        Guid contentPageId,
        Domain.ContentRevision revision,
        Guid expectedVersion,
        CancellationToken cancellationToken)
    {
        Domain.ContentPage? contentPage =
            await contentPageRepository
                .GetByIdAsync(
                    contentPageId,
                    cancellationToken);

        if (contentPage is null)
        {
            throw new NotFoundException(
                "A tartalmi oldal nem található.");
        }

        if (expectedVersion == Guid.Empty ||
            contentPage.Version != expectedVersion)
        {
            throw new ConflictException(
                "A tartalom időközben megváltozott. " +
                "A visszaállítás nem történt meg. " +
                "Töltsd újra az oldalt, és ellenőrizd " +
                "az aktuális tartalmat.");
        }

        contentRevisionRepository.Add(
            Domain.ContentRevision.Capture(
                contentPage));

        contentPage.Update(
            revision.TitleHu,
            revision.TitleEn,
            revision.ContentHu,
            revision.ContentEn);
    }
}
