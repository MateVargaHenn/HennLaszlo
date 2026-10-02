using Microsoft.EntityFrameworkCore;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Infrastructure.Database;

internal sealed class ContentRevisionRepository(
    ContentDbContext dbContext)
    : IContentRevisionRepository
{
    public void Add(
        Domain.ContentRevision revision)
    {
        dbContext.ContentRevisions.Add(
            revision);
    }

    public async Task<
        IReadOnlyList<Domain.ContentRevision>>
        GetByTargetAsync(
            Domain.ContentRevisionTargetType
                targetType,
            Guid targetId,
            CancellationToken cancellationToken =
                default)
    {
        return await dbContext.ContentRevisions
            .AsNoTracking()
            .Where(revision =>
                revision.TargetType ==
                    targetType &&
                revision.TargetId ==
                    targetId)
            .OrderByDescending(revision =>
                revision.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<Domain.ContentRevision?>
        GetByIdAsync(
            Guid revisionId,
            Domain.ContentRevisionTargetType
                targetType,
            Guid targetId,
            CancellationToken cancellationToken =
                default)
    {
        return dbContext.ContentRevisions
            .AsNoTracking()
            .SingleOrDefaultAsync(
                revision =>
                    revision.Id ==
                        revisionId &&
                    revision.TargetType ==
                        targetType &&
                    revision.TargetId ==
                        targetId,
                cancellationToken);
    }
}