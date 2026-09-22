using MediatR;

namespace Modules.Invitation.Application.Invitations.Unpublish;

public sealed record UnpublishInvitationCommand(
    Guid InvitationId)
    : IRequest;