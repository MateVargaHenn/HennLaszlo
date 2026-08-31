namespace Modules.FileStorage.Contracts;

public sealed record FileContentData(
    Stream Content,
    string ContentType);