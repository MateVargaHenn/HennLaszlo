using MediatR;

namespace Modules.Content.Application
    .ContentRevisions.Restore;

public sealed record RestoreContentRevisionCommand(
    Domain.ContentRevisionTargetType
        TargetType,
    Guid TargetId,
    Guid RevisionId)
    : IRequest;