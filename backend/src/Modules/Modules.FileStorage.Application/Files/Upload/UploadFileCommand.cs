using MediatR;

namespace Modules.FileStorage.Application.Files.Upload;

public sealed record UploadFileCommand(
    string FileName,
    string ContentType,
    long SizeInBytes,
    Stream Content) : IRequest<Guid>;