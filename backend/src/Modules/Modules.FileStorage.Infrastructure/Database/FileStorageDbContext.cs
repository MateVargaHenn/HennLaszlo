using Microsoft.EntityFrameworkCore;
using Modules.FileStorage.Domain;

namespace Modules.FileStorage.Infrastructure.Database;

public sealed class FileStorageDbContext(
    DbContextOptions<FileStorageDbContext> options)
    : DbContext(options)
{
    public DbSet<StoredFile> StoredFiles => Set<StoredFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FileStorageDbContext).Assembly);
    }
}