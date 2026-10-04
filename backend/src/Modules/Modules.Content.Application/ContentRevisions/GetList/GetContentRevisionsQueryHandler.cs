using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application
    .ContentRevisions.GetList;

internal sealed class
    GetContentRevisionsQueryHandler(
        IContentRevisionRepository
            contentRevisionRepository)
    : IRequestHandler<
        GetContentRevisionsQuery,
        IReadOnlyList<ContentRevisionListItem>>
{
    public async Task<
        IReadOnlyList<ContentRevisionListItem>>
        Handle(
            GetContentRevisionsQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.ContentRevision>
            revisions =
                await contentRevisionRepository
                    .GetByTargetAsync(
                        request.TargetType,
                        request.TargetId,
                        cancellationToken);

        return revisions
            .Select(revision =>
                new ContentRevisionListItem(
                    revision.Id,
                    revision.TitleHu,
                    revision.CreatedAtUtc))
            .ToList();
    }
}