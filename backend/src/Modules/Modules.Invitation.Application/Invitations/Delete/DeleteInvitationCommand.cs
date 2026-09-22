using MediatR;

namespace Modules.Invitation.Application.Invitations.Delete;

public sealed record DeleteInvitationCommand(
    Guid InvitationId)
    : IRequest;