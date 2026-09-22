using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetImage;

namespace Modules.Artwork.Presentation.Artworks.GetAdminImage;

internal static class GetAdminArtworkImageEndpoint
{
    internal static void MapGetAdminArtworkImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/artworks/{artworkId:guid}/image",
                HandleAsync)
            .WithName("GetAdminArtworkImage")
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
            new GetArtworkImageQuery(
                artworkId,
                IncludeUnpublished: true),
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