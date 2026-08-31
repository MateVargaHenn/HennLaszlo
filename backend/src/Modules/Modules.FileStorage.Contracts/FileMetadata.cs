namespace Modules.FileStorage.Contracts;

public sealed record FileMetadata(
    Guid Id,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes);