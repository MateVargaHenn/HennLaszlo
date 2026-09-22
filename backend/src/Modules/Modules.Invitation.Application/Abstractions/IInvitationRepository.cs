namespace Modules.Invitation.Application.Abstractions;

public interface IInvitationRepository
{
    void Add(Domain.Invitation invitation);

    void Remove(Domain.Invitation invitation);

    Task<Domain.Invitation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Domain.Invitation?> GetPublishedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Invitation>> GetPublishedAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Invitation>> GetAllAsync(
        CancellationToken cancellationToken = default);

}