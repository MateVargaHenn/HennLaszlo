using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application
    .ContentRevisions.GetById;

internal sealed class
    GetContentRevisionByIdQueryHandler(
        IContentRevisionRepository
            contentRevisionRepository)
    : IRequestHandler<
        GetContentRevisionByIdQuery,
        ContentRevisionDetails>
{
    public async Task<ContentRevisionDetails>
        Handle(
            GetContentRevisionByIdQuery request,
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

        return new ContentRevisionDetails(
            revision.Id,
            revision.TitleHu,
            revision.TitleEn,
            revision.SummaryHu,
            revision.SummaryEn,
            revision.ContentHu,
            revision.ContentEn,
            revision.CreatedAtUtc);
    }
}