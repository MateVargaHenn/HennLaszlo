using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.WebsiteSettings.GetAdmin;

namespace Modules.Content.Presentation.WebsiteSettings.GetAdmin;

internal static class GetAdminWebsiteSettingsEndpoint
{
    internal static void MapGetAdminWebsiteSettings(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/website-settings",
                HandleAsync)
            .WithName("GetAdminWebsiteSettings")
            .WithTags("WebsiteSettings")
            .Produces<AdminWebsiteSettingsDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminWebsiteSettingsDetails settings =
            await sender.Send(
                new GetAdminWebsiteSettingsQuery(),
                cancellationToken);

        return Results.Ok(settings);
    }
}
