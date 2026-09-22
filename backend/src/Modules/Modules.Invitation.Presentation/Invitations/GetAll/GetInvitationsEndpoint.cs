using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.GetAll;

namespace Modules.Invitation.Presentation.Invitations.GetAll;

internal static class GetInvitationsEndpoint
{
    internal static void MapGetInvitations(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/invitations",
                HandleAsync)
            .WithName("GetInvitations")
            .WithTags("Invitations")
            .Produces<IReadOnlyList<InvitationListItem>>(
                StatusCodes.Status200OK);
    }

    private static async Task<IResult> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<InvitationListItem> invitations =
            await sender.Send(
                new GetInvitationsQuery(),
                cancellationToken);

        return Results.Ok(invitations);
    }
}