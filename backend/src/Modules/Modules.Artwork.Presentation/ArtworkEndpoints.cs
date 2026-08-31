using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Presentation.Artworks.Create;

namespace Modules.Artwork.Presentation;

public static class ArtworkEndpoints
{
    public static IEndpointRouteBuilder MapArtworkEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateArtwork();

        return endpoints;
    }
}