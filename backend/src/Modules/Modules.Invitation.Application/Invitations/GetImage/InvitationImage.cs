namespace Modules.Invitation.Application.Invitations.GetImage;

public sealed record InvitationImage(
    Stream Content,
    string ContentType);