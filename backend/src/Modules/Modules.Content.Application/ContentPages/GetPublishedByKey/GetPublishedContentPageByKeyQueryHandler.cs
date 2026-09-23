using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentPages.GetPublishedByKey;

internal sealed class GetPublishedContentPageByKeyQueryHandler(
    IContentPageRepository contentPageRepository)
    : IRequestHandler<
        GetPublishedContentPageByKeyQuery,
        PublishedContentPageDetails>
{
    public async Task<PublishedContentPageDetails> Handle(
        GetPublishedContentPageByKeyQuery request,
        CancellationToken cancellationToken)
    {
        Domain.ContentPage? contentPage =
            await contentPageRepository
                .GetPublishedByKeyAsync(
                    request.Key,
                    cancellationToken);

        if (contentPage is null)
        {
            throw new NotFoundException(
                "A tartalmi oldal nem található.");
        }

        return new PublishedContentPageDetails(
            contentPage.Key,
            contentPage.TitleHu,
            contentPage.TitleEn,
            contentPage.ContentHu,
            contentPage.ContentEn);
    }
}