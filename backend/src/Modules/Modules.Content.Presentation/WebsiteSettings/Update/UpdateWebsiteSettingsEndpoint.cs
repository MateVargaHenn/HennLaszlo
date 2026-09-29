using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.WebsiteSettings.Update;

namespace Modules.Content.Presentation.WebsiteSettings.Update;

internal static class UpdateWebsiteSettingsEndpoint
{
    internal static void MapUpdateWebsiteSettings(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/website-settings",
                HandleAsync)
            .WithName("UpdateWebsiteSettings")
            .WithTags("WebsiteSettings")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(
                StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        UpdateWebsiteSettingsRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateWebsiteSettingsCommand(
                request.ArtistName,
                request.ArtistSubtitle,
                request.HeroDescription,
                request.DefaultSeoTitle,
                request.DefaultSeoDescription,
                request.FacebookUrl,
                request.InstagramUrl,
                request.YoutubeUrl),
            cancellationToken);

        return Results.NoContent();
    }
}
