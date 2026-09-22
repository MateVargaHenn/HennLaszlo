using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.Unpublish;

namespace Modules.Artwork.Presentation.Artworks.Unpublish;

internal static class UnpublishArtworkEndpoint
{
    internal static void MapUnpublishArtwork(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/artworks/{artworkId:guid}/unpublish",
                HandleAsync)
            .WithName("UnpublishArtwork")
            .WithTags("Artworks")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UnpublishArtworkCommand(artworkId),
            cancellationToken);

        return Results.NoContent();
    }
}