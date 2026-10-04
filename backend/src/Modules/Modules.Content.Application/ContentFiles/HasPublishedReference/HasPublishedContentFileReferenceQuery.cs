using MediatR;

namespace Modules.Content.Application.ContentFiles.HasPublishedReference;

public sealed record HasPublishedContentFileReferenceQuery(Guid FileId)
    : IRequest<bool>;
