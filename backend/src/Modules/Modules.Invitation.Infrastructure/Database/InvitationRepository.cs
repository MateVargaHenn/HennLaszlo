using Microsoft.EntityFrameworkCore;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Infrastructure.Database;

internal sealed class InvitationRepository(
    InvitationDbContext dbContext)
    : IInvitationRepository
{
    public void Add(Domain.Invitation invitation)
    {
        dbContext.Invitations.Add(invitation);
    }

    public void Remove(Domain.Invitation invitation)
    {
        dbContext.Invitations.Remove(invitation);
    }

    public Task<Domain.Invitation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Invitations.SingleOrDefaultAsync(
            invitation => invitation.Id == id,
            cancellationToken);
    }

    public Task<Domain.Invitation?> GetPublishedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Invitations
            .AsNoTracking()
            .SingleOrDefaultAsync(
                invitation =>
                    invitation.Id == id &&
                    invitation.IsPublished,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Invitation>>
        GetPublishedAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .Where(invitation => invitation.IsPublished)
            .OrderBy(invitation => invitation.DisplayOrder)
            .ThenByDescending(
                invitation => invitation.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Invitation>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .OrderBy(invitation => invitation.DisplayOrder)
            .ThenByDescending(
                invitation => invitation.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}