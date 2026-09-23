namespace Modules.Content.Application.Abstractions;

public interface IContentPageRepository
{
    void Add(Domain.ContentPage contentPage);

    Task<Domain.ContentPage?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Domain.ContentPage?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<Domain.ContentPage?> GetPublishedByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.ContentPage>> GetAllAsync(
        CancellationToken cancellationToken = default);
}