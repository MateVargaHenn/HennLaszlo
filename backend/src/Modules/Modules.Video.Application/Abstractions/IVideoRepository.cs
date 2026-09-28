namespace Modules.Video.Application.Abstractions;

public interface IVideoRepository
{
    void Add(Domain.Video video);

    void Remove(Domain.Video video);

    Task<Domain.Video?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Domain.Video?> GetPublishedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Video>> GetPublishedAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Video>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Domain.Video>>
    GetOrderedForUpdateAsync(
        CancellationToken cancellationToken =
            default);

}