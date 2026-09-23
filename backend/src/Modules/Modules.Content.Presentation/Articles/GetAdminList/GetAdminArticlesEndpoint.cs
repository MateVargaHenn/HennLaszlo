using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.GetAdminList;

namespace Modules.Content.Presentation.Articles.GetAdminList;

internal static class GetAdminArticlesEndpoint
{
    internal static void MapGetAdminArticles(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/articles",
                HandleAsync)
            .WithName("GetAdminArticles")
            .WithTags("Articles")
            .Produces<IReadOnlyList<AdminArticleListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<AdminArticleListItem> articles =
            await sender.Send(
                new GetAdminArticlesQuery(),
                cancellationToken);

        return Results.Ok(articles);
    }
}