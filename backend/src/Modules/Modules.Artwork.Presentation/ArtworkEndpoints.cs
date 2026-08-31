using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Presentation.Artworks.Create;
using Modules.Artwork.Presentation.Artworks.GetAll;

namespace Modules.Artwork.Presentation;

public static class ArtworkEndpoints
{
    public static IEndpointRouteBuilder MapArtworkEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateArtwork();
        endpoints.MapGetArtworks();

        return endpoints;
    }
}