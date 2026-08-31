using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.FileStorage.Application.Files.GetById;

namespace Modules.FileStorage.Presentation.Files.GetById;

internal static class GetFileEndpoint
{
    internal static void MapGetFile(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/files/{id:guid}",
                HandleAsync)
            .WithName("GetFile")
            .WithTags("Files")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        StoredFileContent? file = await sender.Send(
            new GetFileQuery(id),
            cancellationToken);

        if (file is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            file.Content,
            contentType: file.ContentType,
            fileDownloadName: file.OriginalFileName,
            enableRangeProcessing: true);
    }
}