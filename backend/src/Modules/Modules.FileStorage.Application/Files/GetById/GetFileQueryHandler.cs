using MediatR;
using Modules.FileStorage.Application.Abstractions;
using Modules.FileStorage.Domain;

namespace Modules.FileStorage.Application.Files.GetById;

internal sealed class GetFileQueryHandler(
    IStoredFileRepository repository,
    IFileContentStorage contentStorage)
    : IRequestHandler<GetFileQuery, StoredFileContent?>
{
    public async Task<StoredFileContent?> Handle(
        GetFileQuery request,
        CancellationToken cancellationToken)
    {
        StoredFile? storedFile =
            await repository.GetByIdAsync(
                request.Id,
                cancellationToken);

        if (storedFile is null)
        {
            return null;
        }

        Stream? content = await contentStorage.OpenReadAsync(
            storedFile.StorageKey,
            cancellationToken);

        if (content is null)
        {
            return null;
        }

        return new StoredFileContent(
            content,
            storedFile.ContentType,
            storedFile.OriginalFileName);
    }
}