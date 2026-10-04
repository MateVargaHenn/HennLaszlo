using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentFiles.HasPublishedReference;
using Modules.FileStorage.Contracts;

namespace Modules.Content.Presentation.ContentFiles.Get;

internal static class GetContentFileEndpoint
{
    internal static void MapGetContentFile(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/content-files/{fileId:guid}", HandleAsync)
            .WithName("GetContentFile")
            .WithTags("Content")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid fileId,
        HttpContext httpContext,
        ISender sender,
        IFileStorageModule fileStorage,
        CancellationToken cancellationToken)
    {
        // Never let a shared cache expose an admin preview after unpublishing.
        httpContext.Response.Headers.CacheControl = "private, no-store";
        httpContext.Response.Headers.Vary = "Cookie";
        httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";

        // Drafts and unsaved uploads are visible only to the authenticated admin.
        bool isAdmin = httpContext.User.Identity?.IsAuthenticated == true
            && httpContext.User.IsInRole("Admin");

        if (!isAdmin && !await sender.Send(
                new HasPublishedContentFileReferenceQuery(fileId),
                cancellationToken))
        {
            return Results.NotFound();
        }

        FileMetadata? metadata = await fileStorage.GetFileMetadataAsync(
            fileId, cancellationToken);

        if (metadata is null || !IsSupportedType(metadata.ContentType))
        {
            return Results.NotFound();
        }

        FileContentData? file = await fileStorage.GetFileContentAsync(
            fileId, cancellationToken);

        if (file is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            file.Content,
            contentType: metadata.ContentType,
            fileDownloadName: metadata.ContentType == "application/pdf"
                ? metadata.OriginalFileName
                : null,
            enableRangeProcessing: true);
    }

    private static bool IsSupportedType(string contentType)
    {
        return contentType is "image/jpeg" or "image/png" or
            "image/webp" or "image/avif" or "application/pdf";
    }
}
