namespace Modules.Content.Application.Abstractions;

public interface IContentRevisionRepository
{
    void Add(
        Domain.ContentRevision revision);

    Task<IReadOnlyList<Domain.ContentRevision>>
        GetByTargetAsync(
            Domain.ContentRevisionTargetType
                targetType,
            Guid targetId,
            CancellationToken cancellationToken =
                default);

    Task<Domain.ContentRevision?> GetByIdAsync(
        Guid revisionId,
        Domain.ContentRevisionTargetType
            targetType,
        Guid targetId,
        CancellationToken cancellationToken =
            default);
}