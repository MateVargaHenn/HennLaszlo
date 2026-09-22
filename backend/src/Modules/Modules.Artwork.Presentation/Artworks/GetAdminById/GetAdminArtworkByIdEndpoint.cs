using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Application.Artworks.GetAdminById;

namespace Modules.Artwork.Presentation.Artworks.GetAdminById;

internal static class GetAdminArtworkByIdEndpoint
{
    internal static void MapGetAdminArtworkById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/artworks/{artworkId:guid}",
                HandleAsync)
            .WithName("GetAdminArtworkById")
            .WithTags("Artworks")
            .Produces<AdminArtworkDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid artworkId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminArtworkDetails artwork =
            await sender.Send(
                new GetAdminArtworkByIdQuery(
                    artworkId),
                cancellationToken);

        return Results.Ok(artwork);
    }
}