using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.ContentFiles.HasPublishedReference;

internal sealed class HasPublishedContentFileReferenceQueryHandler(
    IArticleRepository articleRepository,
    IContentPageRepository contentPageRepository)
    : IRequestHandler<HasPublishedContentFileReferenceQuery, bool>
{
    public async Task<bool> Handle(
        HasPublishedContentFileReferenceQuery request,
        CancellationToken cancellationToken)
    {
        string filePath = $"/api/content-files/{request.FileId:D}";

        return await articleRepository.HasPublishedFileReferenceAsync(
                filePath, cancellationToken)
            || await contentPageRepository.HasPublishedFileReferenceAsync(
                filePath, cancellationToken);
    }
}
