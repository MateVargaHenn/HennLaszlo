using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.GetAdminById;

namespace Modules.Video.Presentation.Videos.GetAdminById;

internal static class GetAdminVideoByIdEndpoint
{
    internal static void MapGetAdminVideoById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/videos/{videoId:guid}",
                HandleAsync)
            .WithName("GetAdminVideoById")
            .WithTags("Videos")
            .Produces<AdminVideoDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid videoId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminVideoDetails video =
            await sender.Send(
                new GetAdminVideoByIdQuery(
                    videoId),
                cancellationToken);

        return Results.Ok(video);
    }
}