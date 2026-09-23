using Microsoft.EntityFrameworkCore;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Infrastructure.Database;

public sealed class ContentDbContext(
    DbContextOptions<ContentDbContext> options)
    : DbContext(options),
      IContentUnitOfWork
{
    public DbSet<Domain.ContentPage> ContentPages =>
        Set<Domain.ContentPage>();
    
    public DbSet<Domain.Article> Articles =>
    Set<Domain.Article>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("content");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ContentDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(
            cancellationToken);
    }
}