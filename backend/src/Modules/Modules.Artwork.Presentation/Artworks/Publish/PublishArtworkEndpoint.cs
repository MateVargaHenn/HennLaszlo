using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.Publish;

namespace Modules.Artwork.Presentation.Artworks.Publish;

internal static class PublishArtworkEndpoint
{
    internal static void MapPublishArtwork(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/artworks/{artworkId:guid}/publish",
                HandleAsync)
            .WithName("PublishArtwork")
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
            new PublishArtworkCommand(artworkId),
            cancellationToken);

        return Results.NoContent();
    }
}