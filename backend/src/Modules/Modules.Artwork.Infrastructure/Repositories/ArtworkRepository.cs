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
}
