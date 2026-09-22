using MediatR;

namespace Modules.Invitation.Application.Invitations.GetAdminList;

public sealed record GetAdminInvitationsQuery
    : IRequest<IReadOnlyList<AdminInvitationListItem>>;