using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.Create;

namespace Modules.Artwork.Presentation.Artworks.Create;

internal static class CreateArtworkEndpoint
{
    internal static void MapCreateArtwork(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/admin/artworks",
                HandleAsync)
            .WithName("CreateArtwork")
            .WithTags("Artworks")
            .Produces<CreateArtworkResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        CreateArtworkRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateArtworkCommand(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.TechniqueHu,
            request.TechniqueEn,
            request.WidthCm,
            request.HeightCm);

        Guid artworkId = await sender.Send(
            command,
            cancellationToken);

        var response = new CreateArtworkResponse(artworkId);

        return Results.Created(
            $"/api/artworks/{artworkId}",
            response);
    }
}