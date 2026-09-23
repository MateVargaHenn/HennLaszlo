using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.Update;

namespace Modules.Content.Presentation.Articles.Update;

internal static class UpdateArticleEndpoint
{
    internal static void MapUpdateArticle(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/articles/{articleId:guid}",
                HandleAsync)
            .WithName("UpdateArticle")
            .WithTags("Articles")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(
                StatusCodes.Status400BadRequest)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid articleId,
        UpdateArticleRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateArticleCommand(
                articleId,
                request.TitleHu,
                request.TitleEn,
                request.SummaryHu,
                request.SummaryEn,
                request.ContentHu,
                request.ContentEn,
                request.DisplayOrder),
            cancellationToken);

        return Results.NoContent();
    }
}