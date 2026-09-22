using MediatR;

namespace Modules.Invitation.Application.Invitations.GetImage;

public sealed record GetInvitationImageQuery(
    Guid InvitationId)
    : IRequest<InvitationImage?>;