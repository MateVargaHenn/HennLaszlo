using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application.ContentPages.Upsert;

namespace Modules.Content.Presentation.ContentPages.Upsert;

internal static class UpsertContentPageEndpoint
{
    internal static void MapUpsertContentPage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/content-pages/{key}",
                HandleAsync)
            .WithName("UpsertContentPage")
            .WithTags("Content")
            .Produces<UpsertContentPageResponse>(
                StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        string key,
        UpsertContentPageRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        Guid id = await sender.Send(
            new UpsertContentPageCommand(
                key,
                request.TitleHu,
                request.TitleEn,
                request.ContentHu,
                request.ContentEn),
            cancellationToken);

        return Results.Ok(
            new UpsertContentPageResponse(id));
    }
}