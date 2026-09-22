using Microsoft.AspNetCore.Routing;
using Modules.Artwork.Presentation.Artworks.Create;
using Modules.Artwork.Presentation.Artworks.GetAll;
using Modules.Artwork.Presentation.Artworks.AttachImage;
using Modules.Artwork.Presentation.Artworks.Publish;
using Modules.Artwork.Presentation.Artworks.GetImage;
using Modules.Artwork.Presentation.Artworks.GetById;
using Modules.Artwork.Presentation.Artworks.GetAdminList;
using Modules.Artwork.Presentation.Artworks.GetAdminImage;
using Modules.Artwork.Presentation.Artworks.Delete;
using Modules.Artwork.Presentation.Artworks.Update;
using Modules.Artwork.Presentation.Artworks.GetAdminById;

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
        endpoints.MapGetArtworkById();
        endpoints.MapGetAdminArtworks();
        endpoints.MapGetAdminArtworkImage();
        endpoints.MapDeleteArtwork();
        endpoints.MapUpdateArtwork();
        endpoints.MapGetAdminArtworkById();

        return endpoints;
    }
}