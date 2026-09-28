using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.Unpublish;

namespace Modules.Video.Presentation.Videos.Unpublish;

internal static class UnpublishVideoEndpoint
{
    internal static void MapUnpublishVideo(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/videos/{videoId:guid}/unpublish",
                HandleAsync)
            .WithName("UnpublishVideo")
            .WithTags("Videos")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid videoId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UnpublishVideoCommand(
                videoId),
            cancellationToken);

        return Results.NoContent();
    }
}