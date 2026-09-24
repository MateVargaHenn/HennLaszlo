using Microsoft.AspNetCore.Routing;
using Modules.FileStorage.Presentation.Files.Upload;
using Modules.FileStorage.Presentation.Files.GetById;

namespace Modules.FileStorage.Presentation;

public static class FileStorageEndpoints
{
    public static IEndpointRouteBuilder MapAdminFileStorageEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUploadFile();
		endpoints.MapGetFile();

        return endpoints;
    }
}