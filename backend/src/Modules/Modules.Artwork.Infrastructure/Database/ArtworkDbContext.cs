using Microsoft.EntityFrameworkCore;
using Modules.Artwork.Domain;

namespace Modules.Artwork.Infrastructure.Database;

public sealed class ArtworkDbContext(
    DbContextOptions<ArtworkDbContext> options)
    : DbContext(options)
{
    public DbSet<Domain.Artwork> Artworks => Set<Domain.Artwork>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ArtworkDbContext).Assembly);
    }
}