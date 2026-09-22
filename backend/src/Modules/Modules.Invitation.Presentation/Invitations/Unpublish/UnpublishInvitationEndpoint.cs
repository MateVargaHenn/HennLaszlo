using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.Unpublish;

namespace Modules.Invitation.Presentation.Invitations.Unpublish;

internal static class UnpublishInvitationEndpoint
{
    internal static void MapUnpublishInvitation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/invitations/{invitationId:guid}/unpublish",
                HandleAsync)
            .WithName("UnpublishInvitation")
            .WithTags("Invitations")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UnpublishInvitationCommand(
                invitationId),
            cancellationToken);

        return Results.NoContent();
    }
}