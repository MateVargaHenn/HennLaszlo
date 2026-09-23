using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.GetPublishedByKey;

namespace Modules.Content.Presentation.ContentPages.GetPublishedByKey;

internal static class GetPublishedContentPageByKeyEndpoint
{
    internal static void MapGetPublishedContentPageByKey(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/content-pages/{key}",
                HandleAsync)
            .WithName("GetPublishedContentPageByKey")
            .WithTags("Content")
            .Produces<PublishedContentPageDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        string key,
        ISender sender,
        CancellationToken cancellationToken)
    {
        PublishedContentPageDetails contentPage =
            await sender.Send(
                new GetPublishedContentPageByKeyQuery(
                    key),
                cancellationToken);

        return Results.Ok(contentPage);
    }
}