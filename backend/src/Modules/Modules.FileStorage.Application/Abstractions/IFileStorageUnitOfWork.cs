namespace Modules.FileStorage.Application.Abstractions;

public interface IFileStorageUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}