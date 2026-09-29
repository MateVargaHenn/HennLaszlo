using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.WebsiteSettings.GetPublic;

namespace Modules.Content.Presentation.WebsiteSettings.GetPublic;

internal static class GetPublicWebsiteSettingsEndpoint
{
    internal static void MapGetPublicWebsiteSettings(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/website-settings",
                HandleAsync)
            .WithName("GetPublicWebsiteSettings")
            .WithTags("WebsiteSettings")
            .Produces<PublicWebsiteSettings>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        PublicWebsiteSettings settings =
            await sender.Send(
                new GetPublicWebsiteSettingsQuery(),
                cancellationToken);

        return Results.Ok(settings);
    }
}
