using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.Publish;

namespace Modules.Invitation.Presentation.Invitations.Publish;

internal static class PublishInvitationEndpoint
{
    internal static void MapPublishInvitation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/invitations/{invitationId:guid}/publish",
                HandleAsync)
            .WithName("PublishInvitation")
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
            new PublishInvitationCommand(
                invitationId),
            cancellationToken);

        return Results.NoContent();
    }
}