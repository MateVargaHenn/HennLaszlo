using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.GetAdminList;

namespace Modules.Invitation.Presentation.Invitations.GetAdminList;

internal static class GetAdminInvitationsEndpoint
{
    internal static void MapGetAdminInvitations(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/invitations",
                HandleAsync)
            .WithName("GetAdminInvitations")
            .WithTags("Invitations")
            .Produces<
                IReadOnlyList<AdminInvitationListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<AdminInvitationListItem> invitations =
            await sender.Send(
                new GetAdminInvitationsQuery(),
                cancellationToken);

        return Results.Ok(invitations);
    }
}