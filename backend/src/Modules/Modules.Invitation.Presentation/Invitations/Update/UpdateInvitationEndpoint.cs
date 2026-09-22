using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.Update;

namespace Modules.Invitation.Presentation.Invitations.Update;

internal static class UpdateInvitationEndpoint
{
    internal static void MapUpdateInvitation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/api/admin/invitations/{invitationId:guid}",
                HandleAsync)
            .WithName("UpdateInvitation")
            .WithTags("Invitations")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        UpdateInvitationRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateInvitationCommand(
                invitationId,
                request.TitleHu,
                request.TitleEn,
                request.Year,
                request.AltTextHu,
                request.AltTextEn,
                request.DisplayOrder),
            cancellationToken);

        return Results.NoContent();
    }
}