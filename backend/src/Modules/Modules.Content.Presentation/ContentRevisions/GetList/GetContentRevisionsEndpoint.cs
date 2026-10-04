using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Content.Application
    .ContentRevisions.GetById;
using Modules.Content.Application
    .ContentRevisions.GetList;
using Modules.Content.Application.ContentRevisions.Restore;
using Modules.Content.Domain;

namespace Modules.Content.Presentation
    .ContentRevisions.GetList;

internal static class
    GetContentRevisionsEndpoint
{
    internal static void MapGetContentRevisions(
        this IEndpointRouteBuilder endpoints)
    {
        MapTargetRoutes(
            endpoints,
            "articles",
            "Article",
            ContentRevisionTargetType.Article);

        MapTargetRoutes(
            endpoints,
            "content-pages",
            "ContentPage",
            ContentRevisionTargetType.ContentPage);
    }

    private static void MapTargetRoutes(
        IEndpointRouteBuilder endpoints,
        string resourceName,
        string endpointName,
        ContentRevisionTargetType targetType)
    {
        string revisionsRoute =
            $"/api/admin/{resourceName}/" +
            "{targetId:guid}/revisions";

        endpoints.MapGet(
                revisionsRoute,
                (
                    Guid targetId,
                    ISender sender,
                    CancellationToken
                        cancellationToken) =>
                    HandleListAsync(
                        targetType,
                        targetId,
                        sender,
                        cancellationToken))
            .WithName(
                $"Get{endpointName}Revisions")
            .WithTags("Content revisions")
            .Produces<
                IReadOnlyList<
                    ContentRevisionListItem>>(
                        StatusCodes.Status200OK);

        endpoints.MapGet(
                revisionsRoute +
                "/{revisionId:guid}",
                (
                    Guid targetId,
                    Guid revisionId,
                    ISender sender,
                    CancellationToken
                        cancellationToken) =>
                    HandleDetailsAsync(
                        targetType,
                        targetId,
                        revisionId,
                        sender,
                        cancellationToken))
            .WithName(
                $"Get{endpointName}RevisionById")
            .WithTags("Content revisions")
            .Produces<ContentRevisionDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);

		endpoints.MapPost(
			revisionsRoute +
			"/{revisionId:guid}/restore",
			(
				Guid targetId,
				Guid revisionId,
				ISender sender,
				CancellationToken
					cancellationToken) =>
				HandleRestoreAsync(
					targetType,
					targetId,
					revisionId,
					sender,
					cancellationToken))
		.WithName(
			$"Restore{endpointName}Revision")
		.WithTags("Content revisions")
		.Produces(
			StatusCodes.Status204NoContent)
		.ProducesProblem(
			StatusCodes.Status404NotFound);
    }

    private static async Task<IResult>
        HandleListAsync(
            ContentRevisionTargetType targetType,
            Guid targetId,
            ISender sender,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<ContentRevisionListItem>
            revisions =
                await sender.Send(
                    new GetContentRevisionsQuery(
                        targetType,
                        targetId),
                    cancellationToken);

        return Results.Ok(revisions);
    }

    private static async Task<IResult>
        HandleDetailsAsync(
            ContentRevisionTargetType targetType,
            Guid targetId,
            Guid revisionId,
            ISender sender,
            CancellationToken cancellationToken)
    {
        ContentRevisionDetails revision =
            await sender.Send(
                new GetContentRevisionByIdQuery(
                    targetType,
                    targetId,
                    revisionId),
                cancellationToken);

        return Results.Ok(revision);
    }

    private static async Task<IResult>
        HandleRestoreAsync(
            ContentRevisionTargetType targetType,
            Guid targetId,
            Guid revisionId,
            ISender sender,
            CancellationToken cancellationToken)
    {
        await sender.Send(
            new RestoreContentRevisionCommand(
                targetType,
                targetId,
                revisionId),
            cancellationToken);

        return Results.NoContent();
    }
}