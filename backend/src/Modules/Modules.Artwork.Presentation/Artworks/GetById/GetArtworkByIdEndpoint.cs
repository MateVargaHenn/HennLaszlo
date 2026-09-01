using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetById;

namespace Modules.Artwork.Presentation.Artworks.GetById;

internal static class GetArtworkByIdEndpoint
{
    internal static void MapGetArtworkById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/artworks/{artworkId:guid}",
                HandleAsync)
            .WithName("GetArtworkById")
            .WithTags("Artworks")
            .Produces<ArtworkDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetArtworkByIdQuery(artworkId);

        ArtworkDetails artwork = await sender.Send(
            query,
            cancellationToken);

        return Results.Ok(artwork);
    }
}