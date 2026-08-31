namespace Modules.FileStorage.Contracts;

public interface IFileStorageModule
{
    Task<FileMetadata?> GetFileMetadataAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);
}