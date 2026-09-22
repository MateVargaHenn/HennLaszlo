using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.GetAdminById;

namespace Modules.Invitation.Presentation.Invitations.GetAdminById;

internal static class GetAdminInvitationByIdEndpoint
{
    internal static void MapGetAdminInvitationById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/invitations/{invitationId:guid}",
                HandleAsync)
            .WithName("GetAdminInvitationById")
            .WithTags("Invitations")
            .Produces<AdminInvitationDetails>(
                StatusCodes.Status200OK)
            .ProducesProblem(
                StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid invitationId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        AdminInvitationDetails invitation =
            await sender.Send(
                new GetAdminInvitationByIdQuery(
                    invitationId),
                cancellationToken);

        return Results.Ok(invitation);
    }
}