namespace Modules.FileStorage.Contracts;

public interface IFileStorageModule
{
    Task<FileMetadata?> GetFileMetadataAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);

    Task<FileContentData?> GetFileContentAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);

    Task DeleteFileAsync(
        Guid fileId,
        CancellationToken cancellationToken = default);
}