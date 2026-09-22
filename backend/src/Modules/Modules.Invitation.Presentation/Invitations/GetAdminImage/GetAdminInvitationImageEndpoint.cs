using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.GetAdminImage;
using Modules.Invitation.Application.Invitations.GetImage;

namespace Modules.Invitation.Presentation.Invitations.GetAdminImage;

internal static class GetAdminInvitationImageEndpoint
{
    internal static void MapGetAdminInvitationImage(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/admin/invitations/{invitationId:guid}/image",
                HandleAsync)
            .WithName("GetAdminInvitationImage")
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
            new GetAdminInvitationImageQuery(
                invitationId),
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