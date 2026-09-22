using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.AttachImage;

namespace Modules.Invitation.Presentation.Invitations.AttachImage;

internal static class AttachInvitationImageEndpoint
{
    internal static void MapAttachInvitationImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/invitations/{invitationId:guid}/image",
                HandleAsync)
            .WithName("AttachInvitationImage")
            .WithTags("Invitations")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        AttachInvitationImageRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new AttachInvitationImageCommand(
                invitationId,
                request.FileId),
            cancellationToken);

        return Results.NoContent();
    }
}