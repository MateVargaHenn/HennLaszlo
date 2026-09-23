using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.GetPublishedList;

namespace Modules.Content.Presentation.Articles.GetPublishedList;

internal static class GetPublishedArticlesEndpoint
{
    internal static void MapGetPublishedArticles(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/articles",
                HandleAsync)
            .WithName("GetPublishedArticles")
            .WithTags("Articles")
            .Produces<
                IReadOnlyList<PublishedArticleListItem>>(
                    StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<PublishedArticleListItem> articles =
            await sender.Send(
                new GetPublishedArticlesQuery(),
                cancellationToken);

        return Results.Ok(articles);
    }
}