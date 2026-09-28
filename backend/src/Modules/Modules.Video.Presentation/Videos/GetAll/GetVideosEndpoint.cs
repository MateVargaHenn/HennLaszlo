using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.GetAll;

namespace Modules.Video.Presentation.Videos.GetAll;

internal static class GetVideosEndpoint
{
    internal static void MapGetVideos(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/videos",
                HandleAsync)
            .WithName("GetVideos")
            .WithTags("Videos")
            .Produces<IReadOnlyList<VideoListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<VideoListItem> videos =
            await sender.Send(
                new GetVideosQuery(),
                cancellationToken);

        return Results.Ok(videos);
    }
}