using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.AttachImage;

namespace Modules.Artwork.Presentation.Artworks.AttachImage;

internal static class AttachArtworkImageEndpoint
{
    internal static void MapAttachArtworkImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/artworks/{artworkId:guid}/image",
                HandleAsync)
            .WithName("AttachArtworkImage")
            .WithTags("Artworks")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        AttachArtworkImageRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new AttachArtworkImageCommand(
                artworkId,
                request.FileId),
            cancellationToken);

        return Results.NoContent();
    }
}