using MediatR;
using Modules.Invitation.Application.Invitations.GetImage;

namespace Modules.Invitation.Application.Invitations.GetAdminImage;

public sealed record GetAdminInvitationImageQuery(
    Guid InvitationId)
    : IRequest<InvitationImage?>;