using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.Unpublish;

namespace Modules.Content.Presentation.Articles.Unpublish;

internal static class UnpublishArticleEndpoint
{
    internal static void MapUnpublishArticle(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/articles/{articleId:guid}/unpublish",
                HandleAsync)
            .WithName("UnpublishArticle")
            .WithTags("Articles")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid articleId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UnpublishArticleCommand(articleId),
            cancellationToken);

        return Results.NoContent();
    }
}