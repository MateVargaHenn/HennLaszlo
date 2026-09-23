using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.Create;

namespace Modules.Content.Presentation.Articles.Create;

internal static class CreateArticleEndpoint
{
    internal static void MapCreateArticle(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/admin/articles",
                HandleAsync)
            .WithName("CreateArticle")
            .WithTags("Articles")
            .Produces<CreateArticleResponse>(
                StatusCodes.Status200OK)
            .ProducesValidationProblem(
                StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        CreateArticleRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        Guid articleId = await sender.Send(
            new CreateArticleCommand(
                request.Slug,
                request.TitleHu,
                request.TitleEn,
                request.SummaryHu,
                request.SummaryEn,
                request.ContentHu,
                request.ContentEn,
                request.DisplayOrder),
            cancellationToken);

        return Results.Ok(
            new CreateArticleResponse(articleId));
    }
}