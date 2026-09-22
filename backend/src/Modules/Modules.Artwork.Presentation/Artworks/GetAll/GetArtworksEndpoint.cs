using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetAll;

namespace Modules.Artwork.Presentation.Artworks.GetAll;

internal static class GetArtworksEndpoint
{
    internal static void MapGetArtworks(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/artworks",
                HandleAsync)
            .WithName("GetArtworks")
            .WithTags("Artworks")
            .Produces<IReadOnlyCollection<ArtworkListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<ArtworkListItem> artworks =
            await sender.Send(
                new GetArtworksQuery(),
                cancellationToken);

        return Results.Ok(artworks);
    }
}