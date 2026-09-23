using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.Delete;

namespace Modules.Content.Presentation.Articles.Delete;

internal static class DeleteArticleEndpoint
{
    internal static void MapDeleteArticle(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/admin/articles/{articleId:guid}",
                HandleAsync)
            .WithName("DeleteArticle")
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
            new DeleteArticleCommand(articleId),
            cancellationToken);

        return Results.NoContent();
    }
}