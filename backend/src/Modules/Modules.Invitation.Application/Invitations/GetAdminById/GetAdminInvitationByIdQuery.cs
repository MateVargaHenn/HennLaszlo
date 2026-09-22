using MediatR;

namespace Modules.Invitation.Application.Invitations.GetAdminById;

public sealed record GetAdminInvitationByIdQuery(
    Guid InvitationId)
    : IRequest<AdminInvitationDetails>;