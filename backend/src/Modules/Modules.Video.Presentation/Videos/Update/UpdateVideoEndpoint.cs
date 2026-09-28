using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.Update;

namespace Modules.Video.Presentation.Videos.Update;

internal static class UpdateVideoEndpoint
{
    internal static void MapUpdateVideo(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/videos/{videoId:guid}",
                HandleAsync)
            .WithName("UpdateVideo")
            .WithTags("Videos")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid videoId,
        UpdateVideoRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateVideoCommand(
                videoId,
                request.TitleHu,
                request.TitleEn,
                request.Year,
                request.DescriptionHu,
                request.DescriptionEn,
                request.VideoUrl,
                request.DisplayOrder),
            cancellationToken);

        return Results.NoContent();
    }
}