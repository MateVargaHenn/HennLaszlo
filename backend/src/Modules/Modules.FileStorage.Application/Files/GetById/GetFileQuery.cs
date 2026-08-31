using MediatR;

namespace Modules.FileStorage.Application.Files.GetById;

public sealed record GetFileQuery(Guid Id)
    : IRequest<StoredFileContent?>;