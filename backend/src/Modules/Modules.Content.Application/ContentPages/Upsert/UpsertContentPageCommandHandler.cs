using MediatR;
using Modules.Content.Application.Abstractions;

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