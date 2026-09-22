using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.Delete;

namespace Modules.Invitation.Presentation.Invitations.Delete;

internal static class DeleteInvitationEndpoint
{
    internal static void MapDeleteInvitation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/admin/invitations/{invitationId:guid}",
                HandleAsync)
            .WithName("DeleteInvitation")
            .WithTags("Invitations")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteInvitationCommand(invitationId),
            cancellationToken);

        return Results.NoContent();
    }
}