using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Application.Invitations.Create;

namespace Modules.Invitation.Presentation.Invitations.Create;

internal static class CreateInvitationEndpoint
{
    internal static void MapCreateInvitation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/admin/invitations",
                HandleAsync)
            .WithName("CreateInvitation")
            .WithTags("Invitations")
            .Produces<CreateInvitationResponse>(
                StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> HandleAsync(
        CreateInvitationRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateInvitationCommand(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.AltTextHu,
            request.AltTextEn,
            request.DisplayOrder);

        Guid invitationId = await sender.Send(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/admin/invitations/{invitationId}",
            new CreateInvitationResponse(invitationId));
    }
}