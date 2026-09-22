namespace Modules.Artwork.Application.Abstractions;

public interface IArtworkUnitOfWork
{
	Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
