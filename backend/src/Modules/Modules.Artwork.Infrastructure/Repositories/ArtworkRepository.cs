using Microsoft.EntityFrameworkCore;
using Modules.Artwork.Application.Abstractions;
using Modules.Artwork.Infrastructure.Database;

namespace Modules.Artwork.Infrastructure.Repositories;

public class ArtworkRepository(
    ArtworkDbContext dbContext)
    : IArtworkRepository
{
	public void Add(Domain.Artwork artwork)
    {
        dbContext.Artworks.Add(artwork);
    }

    public async Task<IReadOnlyCollection<Domain.Artwork>> GetPublishedAsync(
    CancellationToken cancellationToken = default)
    {
        return await dbContext.Artworks
            .AsNoTracking()
            .Where(artwork => artwork.IsPublished)
            .OrderBy(artwork => artwork.DisplayOrder)
            .ThenByDescending(artwork => artwork.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<Domain.Artwork?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        return dbContext.Artworks.SingleOrDefaultAsync(
            artwork => artwork.Id == id,
            cancellationToken);
    }
}
