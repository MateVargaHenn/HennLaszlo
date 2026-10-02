using MediatR;

namespace Modules.Content.Application
    .ContentRevisions.GetList;

public sealed record GetContentRevisionsQuery(
    Domain.ContentRevisionTargetType TargetType,
    Guid TargetId)
    : IRequest<
        IReadOnlyList<ContentRevisionListItem>>;