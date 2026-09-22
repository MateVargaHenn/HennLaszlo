using MediatR;

namespace Modules.Invitation.Application.Invitations.GetAll;

public sealed record GetInvitationsQuery
    : IRequest<IReadOnlyList<InvitationListItem>>;