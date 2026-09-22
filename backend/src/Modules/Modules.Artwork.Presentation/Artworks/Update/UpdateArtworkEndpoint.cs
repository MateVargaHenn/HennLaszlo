using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.Update;

namespace Modules.Artwork.Presentation.Artworks.Update;

internal static class UpdateArtworkEndpoint
{
    internal static void MapUpdateArtwork(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/artworks/{artworkId:guid}",
                HandleAsync)
            .WithName("UpdateArtwork")
            .WithTags("Artworks")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        UpdateArtworkRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArtworkCommand(
            artworkId,
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.TechniqueHu,
            request.TechniqueEn,
            request.WidthCm,
            request.HeightCm,
            request.DescriptionHu,
            request.DescriptionEn,
            request.IsFeatured,
            request.DisplayOrder);

        await sender.Send(
            command,
            cancellationToken);

        return Results.NoContent();
    }
}