using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.GetPublishedBySlug;

namespace Modules.Content.Presentation.Articles.GetPublishedBySlug;

internal static class GetPublishedArticleBySlugEndpoint
{
    internal static void MapGetPublishedArticleBySlug(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/articles/{slug}",
                HandleAsync)
            .WithName("GetPublishedArticleBySlug")
            .WithTags("Articles")
            .Produces<PublishedArticleDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        string slug,
        ISender sender,
        CancellationToken cancellationToken)
    {
        PublishedArticleDetails article =
            await sender.Send(
                new GetPublishedArticleBySlugQuery(slug),
                cancellationToken);

        return Results.Ok(article);
    }
}