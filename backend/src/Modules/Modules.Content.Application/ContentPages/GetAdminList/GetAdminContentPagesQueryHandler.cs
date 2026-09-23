using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentPages.GetAdminList;

internal sealed class GetAdminContentPagesQueryHandler(
    IContentPageRepository contentPageRepository)
    : IRequestHandler<
        GetAdminContentPagesQuery,
        IReadOnlyList<AdminContentPageListItem>>
{
    public async Task<
        IReadOnlyList<AdminContentPageListItem>> Handle(
            GetAdminContentPagesQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.ContentPage> contentPages =
            await contentPageRepository.GetAllAsync(
                cancellationToken);

        return contentPages
            .Select(contentPage =>
                new AdminContentPageListItem(
                    contentPage.Id,
                    contentPage.Key,
                    contentPage.TitleHu,
                    contentPage.TitleEn,
                    contentPage.IsPublished,
                    contentPage.UpdatedAtUtc))
            .ToList();
    }
}