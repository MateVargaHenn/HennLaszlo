using Microsoft.AspNetCore.Routing;
using Modules.Video.Presentation.Videos.Create;
using Modules.Video.Presentation.Videos.Delete;
using Modules.Video.Presentation.Videos.GetAdminById;
using Modules.Video.Presentation.Videos.GetAdminList;
using Modules.Video.Presentation.Videos.GetAll;
using Modules.Video.Presentation.Videos.Publish;
using Modules.Video.Presentation.Videos.Unpublish;
using Modules.Video.Presentation.Videos.Update;

namespace Modules.Video.Presentation;

public static class VideoEndpoints
{
    public static IEndpointRouteBuilder
        MapPublicVideoEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetVideos();

        return endpoints;
    }

    public static IEndpointRouteBuilder
        MapAdminVideoEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateVideo();
        endpoints.MapPublishVideo();
        endpoints.MapUnpublishVideo();
        endpoints.MapGetAdminVideos();
        endpoints.MapGetAdminVideoById();
        endpoints.MapUpdateVideo();
        endpoints.MapDeleteVideo();

        return endpoints;
    }
}