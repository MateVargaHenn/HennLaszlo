namespace Modules.Invitation.Application.Abstractions;

public interface IInvitationUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}