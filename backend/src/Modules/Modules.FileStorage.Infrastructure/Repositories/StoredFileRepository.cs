using Modules.FileStorage.Application.Abstractions;
using Modules.FileStorage.Domain;
using Modules.FileStorage.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Modules.FileStorage.Infrastructure.Repositories;

internal sealed class StoredFileRepository(
    FileStorageDbContext dbContext)
    : IStoredFileRepository
{
    public void Add(StoredFile storedFile)
    {
        dbContext.StoredFiles.Add(storedFile);
    }

	public Task<StoredFile?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default)
	{
		return dbContext.StoredFiles
			.AsNoTracking()
			.SingleOrDefaultAsync(
				storedFile => storedFile.Id == id,
				cancellationToken);
	}

    public void Remove(StoredFile storedFile)
    {
        dbContext.StoredFiles.Remove(storedFile);
    }
}