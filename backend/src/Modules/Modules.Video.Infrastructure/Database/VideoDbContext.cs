using Microsoft.EntityFrameworkCore;
using Modules.Video.Application.Abstractions;

namespace Modules.Video.Infrastructure.Database;

public sealed class VideoDbContext(
    DbContextOptions<VideoDbContext> options)
    : DbContext(options),
      IVideoUnitOfWork
{
    public DbSet<Domain.Video> Videos =>
        Set<Domain.Video>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(VideoDbContext).Assembly);
    }
}