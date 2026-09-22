using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.Delete;

namespace Modules.Artwork.Presentation.Artworks.Delete;

internal static class DeleteArtworkEndpoint
{
    internal static void MapDeleteArtwork(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/admin/artworks/{artworkId:guid}",
                HandleAsync)
            .WithName("DeleteArtwork")
            .WithTags("Artworks")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteArtworkCommand(artworkId),
            cancellationToken);

        return Results.NoContent();
    }
}