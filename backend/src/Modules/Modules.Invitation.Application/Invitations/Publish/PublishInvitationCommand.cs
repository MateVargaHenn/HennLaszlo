using MediatR;

namespace Modules.Invitation.Application.Invitations.Publish;

public sealed record PublishInvitationCommand(
    Guid InvitationId)
    : IRequest;