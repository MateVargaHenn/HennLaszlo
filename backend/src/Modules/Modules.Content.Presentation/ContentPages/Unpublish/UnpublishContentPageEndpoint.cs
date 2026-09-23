using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.Unpublish;

namespace Modules.Content.Presentation.ContentPages.Unpublish;

internal static class UnpublishContentPageEndpoint
{
    internal static void MapUnpublishContentPage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/content-pages/{key}/unpublish",
                HandleAsync)
            .WithName("UnpublishContentPage")
            .WithTags("Content")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        string key,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UnpublishContentPageCommand(key),
            cancellationToken);

        return Results.NoContent();
    }
}