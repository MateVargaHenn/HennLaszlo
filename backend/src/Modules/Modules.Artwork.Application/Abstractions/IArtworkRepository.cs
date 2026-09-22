namespace Modules.Artwork.Application.Abstractions;

public interface IArtworkRepository
{
	void Add(Domain.Artwork artwork);

	Task<IReadOnlyCollection<Domain.Artwork>> GetPublishedAsync(
    CancellationToken cancellationToken = default);

	Task<Domain.Artwork?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default);

    Task<Domain.Artwork?> GetPublishedByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Artwork>> GetAllAsync(
    CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Artwork>> GetFeaturedAsync(
    CancellationToken cancellationToken = default);

    void Remove(Domain.Artwork artwork);
}
