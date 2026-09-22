using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.GetImage;

namespace Modules.Invitation.Presentation.Invitations.GetImage;

internal static class GetInvitationImageEndpoint
{
    internal static void MapGetInvitationImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/invitations/{invitationId:guid}/image",
                HandleAsync)
            .WithName("GetInvitationImage")
            .WithTags("Invitations")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        InvitationImage? image = await sender.Send(
            new GetInvitationImageQuery(invitationId),
            cancellationToken);

        if (image is null)
        {
            return Results.NotFound();
        }

        return Results.File(
            image.Content,
            contentType: image.ContentType,
            enableRangeProcessing: true);
    }
}