using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetImage;

namespace Modules.Artwork.Presentation.Artworks.GetImage;

internal static class GetArtworkImageEndpoint
{
    internal static void MapGetArtworkImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/artworks/{artworkId:guid}/image",
                HandleAsync)
            .WithName("GetArtworkImage")
            .WithTags("Artworks")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        ArtworkImage? image = await sender.Send(
            new GetArtworkImageQuery(artworkId),
            cancellationToken);

        if (image is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            image.Content,
            contentType: image.ContentType,
            enableRangeProcessing: true);
    }
}