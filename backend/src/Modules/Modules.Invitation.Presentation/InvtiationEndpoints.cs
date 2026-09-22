using Microsoft.AspNetCore.Routing;
using Modules.Invitation.Presentation.Invitations.Create;
using Modules.Invitation.Presentation.Invitations.AttachImage;
using Modules.Invitation.Presentation.Invitations.Publish;
using Modules.Invitation.Presentation.Invitations.Unpublish;
using Modules.Invitation.Presentation.Invitations.GetAll;
using Modules.Invitation.Presentation.Invitations.GetImage;
using Modules.Invitation.Presentation.Invitations.GetAdminList;
using Modules.Invitation.Presentation.Invitations.GetAdminImage;
using Modules.Invitation.Presentation.Invitations.GetAdminById;
using Modules.Invitation.Presentation.Invitations.Update;
using Modules.Invitation.Presentation.Invitations.Delete;

namespace Modules.Invitation.Presentation;

public static class InvitationEndpoints
{
    public static IEndpointRouteBuilder MapInvitationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateInvitation();
        endpoints.MapAttachInvitationImage();
        endpoints.MapPublishInvitation();
        endpoints.MapUnpublishInvitation();
        endpoints.MapGetInvitations();
        endpoints.MapGetInvitationImage();
        endpoints.MapGetAdminInvitations(); 
        endpoints.MapGetAdminInvitationImage();
        endpoints.MapGetAdminInvitationById();
        endpoints.MapUpdateInvitation();
        endpoints.MapDeleteInvitation();

        return endpoints;
    }
}