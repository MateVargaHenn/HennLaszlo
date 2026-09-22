using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.FileStorage.Application.Files.Upload;

namespace Modules.FileStorage.Presentation.Files.Upload;

internal static class UploadFileEndpoint
{
    internal static void MapUploadFile(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/admin/files",
                HandleAsync)
            .WithName("UploadFile")
            .WithTags("Files")
            .DisableAntiforgery()
            .Produces<UploadFileResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        IFormFile file,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await using Stream content =
            file.OpenReadStream();

        var command = new UploadFileCommand(
            file.FileName,
            file.ContentType,
            file.Length,
            content);

        Guid fileId = await sender.Send(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/files/{fileId}",
            new UploadFileResponse(fileId));
    }
}