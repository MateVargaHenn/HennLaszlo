using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.Articles.GetAdminById;

namespace Modules.Content.Presentation.Articles.GetAdminById;

internal static class GetAdminArticleByIdEndpoint
{
    internal static void MapGetAdminArticleById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/articles/{articleId:guid}",
                HandleAsync)
            .WithName("GetAdminArticleById")
            .WithTags("Articles")
            .Produces<AdminArticleDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid articleId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminArticleDetails article =
            await sender.Send(
                new GetAdminArticleByIdQuery(articleId),
                cancellationToken);

        return Results.Ok(article);
    }
}