using MediatR;

namespace Modules.Invitation.Application.Invitations.AttachImage;

public sealed record AttachInvitationImageCommand(
    Guid InvitationId,
    Guid FileId)
    : IRequest;