using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Video.Application.Videos.Create;

namespace Modules.Video.Presentation.Videos.Create;

internal static class CreateVideoEndpoint
{
    internal static void MapCreateVideo(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/admin/videos",
                HandleAsync)
            .WithName("CreateVideo")
            .WithTags("Videos")
            .Produces<CreateVideoResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        CreateVideoRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateVideoCommand(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.DescriptionHu,
            request.DescriptionEn,
            request.VideoUrl,
            request.DisplayOrder);

        Guid videoId = await sender.Send(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/admin/videos/{videoId}",
            new CreateVideoResponse(videoId));
    }
}