using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Presentation.Artworks.Create;
using Modules.Artwork.Presentation.Artworks.GetAll;
using Modules.Artwork.Presentation.Artworks.AttachImage;
using Modules.Artwork.Presentation.Artworks.Publish;
using Modules.Artwork.Presentation.Artworks.GetImage;

namespace Modules.Artwork.Presentation;

public static class ArtworkEndpoints
{
    public static IEndpointRouteBuilder MapArtworkEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateArtwork();
        endpoints.MapGetArtworks();
        endpoints.MapAttachArtworkImage();
        endpoints.MapPublishArtwork();
        endpoints.MapGetArtworkImage();

        return endpoints;
    }
}