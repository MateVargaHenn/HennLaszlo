using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentPages.Unpublish;

internal sealed class UnpublishContentPageCommandHandler(
    IContentPageRepository contentPageRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<UnpublishContentPageCommand>
{
    public async Task Handle(
        UnpublishContentPageCommand request,
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

        contentPage.Unpublish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}