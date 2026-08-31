using MediatR;
using Modules.FileStorage.Application.Abstractions;
using Modules.FileStorage.Domain;

namespace Modules.FileStorage.Application.Files.Upload;

internal sealed class UploadFileCommandHandler(
    IFileContentStorage contentStorage,
    IStoredFileRepository repository,
    IFileStorageUnitOfWork unitOfWork)
    : IRequestHandler<UploadFileCommand, Guid>
{
    public async Task<Guid> Handle(
        UploadFileCommand request,
        CancellationToken cancellationToken)
    {
        string extension =
            Path.GetExtension(request.FileName);

        string storageKey = await contentStorage.SaveAsync(
            request.Content,
            extension,
            cancellationToken);

        try
        {
            StoredFile storedFile = StoredFile.Create(
                request.FileName,
                storageKey,
                request.ContentType,
                request.SizeInBytes);

            repository.Add(storedFile);

            await unitOfWork.SaveChangesAsync(
                cancellationToken);

            return storedFile.Id;
        }
        catch
        {
            await contentStorage.DeleteAsync(
                storageKey,
                CancellationToken.None);

            throw;
        }
    }
}