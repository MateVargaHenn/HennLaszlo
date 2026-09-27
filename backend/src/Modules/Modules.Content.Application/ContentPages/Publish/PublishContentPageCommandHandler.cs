using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentPages.Publish;

internal sealed class PublishContentPageCommandHandler(
    IContentPageRepository contentPageRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<PublishContentPageCommand>
{
    public async Task Handle(
        PublishContentPageCommand request,
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

        contentPage.Publish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}