using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentPages.GetAdminByKey;

internal sealed class GetAdminContentPageByKeyQueryHandler(
    IContentPageRepository contentPageRepository)
    : IRequestHandler<
        GetAdminContentPageByKeyQuery,
        AdminContentPageDetails>
{
    public async Task<AdminContentPageDetails> Handle(
        GetAdminContentPageByKeyQuery request,
        CancellationToken cancellationToken)
    {
        Domain.ContentPage? contentPage =
            await contentPageRepository.GetByKeyAsync(
                request.Key,
                cancellationToken);

        if (contentPage is null)
        {
            throw new NotFoundException(
                "A tartalmi oldal nem található.");
        }

        return new AdminContentPageDetails(
            contentPage.Id,
            contentPage.Key,
            contentPage.TitleHu,
            contentPage.TitleEn,
            contentPage.ContentHu,
            contentPage.ContentEn,
            contentPage.IsPublished,
            contentPage.UpdatedAtUtc);
    }
}