using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.GetAdminList;

namespace Modules.Video.Presentation.Videos.GetAdminList;

internal static class GetAdminVideosEndpoint
{
    internal static void MapGetAdminVideos(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/videos",
                HandleAsync)
            .WithName("GetAdminVideos")
            .WithTags("Videos")
            .Produces<
                IReadOnlyList<AdminVideoListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<AdminVideoListItem> videos =
            await sender.Send(
                new GetAdminVideosQuery(),
                cancellationToken);

        return Results.Ok(videos);
    }
}