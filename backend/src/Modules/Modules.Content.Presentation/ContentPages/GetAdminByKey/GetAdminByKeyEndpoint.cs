using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.GetAdminByKey;

namespace Modules.Content.Presentation.ContentPages.GetAdminByKey;

internal static class GetAdminContentPageByKeyEndpoint
{
    internal static void MapGetAdminContentPageByKey(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/content-pages/{key}",
                HandleAsync)
            .WithName("GetAdminContentPageByKey")
            .WithTags("Content")
            .Produces<AdminContentPageDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        string key,
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminContentPageDetails contentPage =
            await sender.Send(
                new GetAdminContentPageByKeyQuery(key),
                cancellationToken);

        return Results.Ok(contentPage);
    }
}