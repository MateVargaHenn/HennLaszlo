using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.Publish;

namespace Modules.Content.Presentation.Articles.Publish;

internal static class PublishArticleEndpoint
{
    internal static void MapPublishArticle(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/articles/{articleId:guid}/publish",
                HandleAsync)
            .WithName("PublishArticle")
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
            new PublishArticleCommand(articleId),
            cancellationToken);

        return Results.NoContent();
    }
}