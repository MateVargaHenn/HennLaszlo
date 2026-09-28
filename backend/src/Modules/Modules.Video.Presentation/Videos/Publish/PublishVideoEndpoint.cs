using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.Publish;

namespace Modules.Video.Presentation.Videos.Publish;

internal static class PublishVideoEndpoint
{
    internal static void MapPublishVideo(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/videos/{videoId:guid}/publish",
                HandleAsync)
            .WithName("PublishVideo")
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
            new PublishVideoCommand(
                videoId),
            cancellationToken);

        return Results.NoContent();
    }
}