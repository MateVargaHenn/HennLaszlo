using MediatR;

namespace Modules.Content.Application
    .ContentRevisions.GetById;

public sealed record
    GetContentRevisionByIdQuery(
        Domain.ContentRevisionTargetType
            TargetType,
        Guid TargetId,
        Guid RevisionId)
    : IRequest<ContentRevisionDetails>;