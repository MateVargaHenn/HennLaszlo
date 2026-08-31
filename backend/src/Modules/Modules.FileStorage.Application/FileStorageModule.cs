using Modules.FileStorage.Application.Abstractions;
using Modules.FileStorage.Contracts;
using Modules.FileStorage.Domain;

namespace Modules.FileStorage.Application;

internal sealed class FileStorageModule(
    IStoredFileRepository repository)
    : IFileStorageModule
{
    public async Task<FileMetadata?> GetFileMetadataAsync(
        Guid fileId,
        CancellationToken cancellationToken = default)
    {
        StoredFile? storedFile =
            await repository.GetByIdAsync(
                fileId,
                cancellationToken);

        if (storedFile is null)
        {
            return null;
        }

        return new FileMetadata(
            storedFile.Id,
            storedFile.OriginalFileName,
            storedFile.ContentType,
            storedFile.SizeInBytes);
    }
}