using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.Publish;

namespace Modules.Content.Presentation.ContentPages.Publish;

internal static class PublishContentPageEndpoint
{
    internal static void MapPublishContentPage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/content-pages/{key}/publish",
                HandleAsync)
            .WithName("PublishContentPage")
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
            new PublishContentPageCommand(key),
            cancellationToken);

        return Results.NoContent();
    }
}