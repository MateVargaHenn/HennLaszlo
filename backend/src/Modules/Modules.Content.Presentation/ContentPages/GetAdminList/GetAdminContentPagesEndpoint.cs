using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.GetAdminList;

namespace Modules.Content.Presentation.ContentPages.GetAdminList;

internal static class GetAdminContentPagesEndpoint
{
    internal static void MapGetAdminContentPages(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/content-pages",
                HandleAsync)
            .WithName("GetAdminContentPages")
            .WithTags("Content")
            .Produces<IReadOnlyList<AdminContentPageListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<AdminContentPageListItem> contentPages =
            await sender.Send(
                new GetAdminContentPagesQuery(),
                cancellationToken);

        return Results.Ok(contentPages);
    }
}