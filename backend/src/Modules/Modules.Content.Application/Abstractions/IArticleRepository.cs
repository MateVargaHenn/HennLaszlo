namespace Modules.Content.Application.Abstractions;

public interface IArticleRepository
{
    void Add(Domain.Article article);

    void Remove(Domain.Article article);

    Task<Domain.Article?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Domain.Article?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Article>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Article>> GetPublishedAsync(
        CancellationToken cancellationToken = default);

    Task<Domain.Article?> GetPublishedBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);
}