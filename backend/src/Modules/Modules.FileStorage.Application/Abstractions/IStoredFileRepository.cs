using Modules.FileStorage.Domain;

namespace Modules.FileStorage.Application.Abstractions;

public interface IStoredFileRepository
{
    void Add(StoredFile storedFile);

	Task<StoredFile?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken = default);

    void Remove(StoredFile storedFile);
}