namespace Modules.FileStorage.Application.Files.GetById;

public sealed record StoredFileContent(
    Stream Content,
    string ContentType,
    string OriginalFileName);