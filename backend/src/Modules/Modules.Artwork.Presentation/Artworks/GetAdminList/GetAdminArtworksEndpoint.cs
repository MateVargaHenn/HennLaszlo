using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetAdminList;

namespace Modules.Artwork.Presentation.Artworks.GetAdminList;

internal static class GetAdminArtworksEndpoint
{
    internal static void MapGetAdminArtworks(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/artworks",
                HandleAsync)
            .WithName("GetAdminArtworks")
            .WithTags("Artworks")
            .Produces<IReadOnlyList<AdminArtworkListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAdminArtworksQuery();

        IReadOnlyList<AdminArtworkListItem> artworks =
            await sender.Send(
                query,
                cancellationToken);

        return Results.Ok(artworks);
    }
}