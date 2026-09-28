using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.Delete;

namespace Modules.Video.Presentation.Videos.Delete;

internal static class DeleteVideoEndpoint
{
    internal static void MapDeleteVideo(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/admin/videos/{videoId:guid}",
                HandleAsync)
            .WithName("DeleteVideo")
            .WithTags("Videos")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<IResult> HandleAsync(
        Guid videoId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteVideoCommand(videoId),
            cancellationToken);

        return Results.NoContent();
    }
}