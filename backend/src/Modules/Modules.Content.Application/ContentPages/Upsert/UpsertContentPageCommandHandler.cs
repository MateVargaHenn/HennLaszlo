using MediatR;
using Modules.Content.Application.Abstractions;
using BuildingBlocks.Application.Exceptions;

namespace Modules.Content.Application.ContentPages.Upsert;

internal sealed class UpsertContentPageCommandHandler(
    IContentPageRepository contentPageRepository,
    IContentRevisionRepository
        contentRevisionRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<
        UpsertContentPageCommand,
        Guid>
{
    public async Task<Guid> Handle(
        UpsertContentPageCommand request,
        CancellationToken cancellationToken)
    {
        Domain.ContentPage? contentPage =
            await contentPageRepository.GetByKeyAsync(
                request.Key,
                cancellationToken);

        if (contentPage is null)
        {
            contentPage = Domain.ContentPage.Create(
                request.Key,
                request.TitleHu,
                request.TitleEn,
                request.ContentHu,
                request.ContentEn);

            contentPageRepository.Add(contentPage);
        }
        else
        {
            if (request.ExpectedVersion is null ||
                contentPage.Version != request.ExpectedVersion)
            {
                throw new ConflictException(
                    "A tartalmi oldal időközben megszűnt. " +
                    "A módosításaid nem kerültek mentésre. " +
                    "Másold ki őket, mielőtt újratöltöd az oldalt.");
            }
            contentRevisionRepository.Add(
                Domain.ContentRevision.Capture(
                    contentPage));

            contentPage.Update(
                request.TitleHu,
                request.TitleEn,
                request.ContentHu,
                request.ContentEn);
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return contentPage.Id;
    }
}