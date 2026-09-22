using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.GetAll;

internal sealed class GetInvitationsQueryHandler(
    IInvitationRepository invitationRepository)
    : IRequestHandler<
        GetInvitationsQuery,
        IReadOnlyList<InvitationListItem>>
{
    public async Task<IReadOnlyList<InvitationListItem>> Handle(
        GetInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Invitation> invitations =
            await invitationRepository.GetPublishedAsync(
                cancellationToken);

        return invitations
            .Select(invitation => new InvitationListItem(
                invitation.Id,
                invitation.TitleHu,
                invitation.TitleEn,
                invitation.Year,
                invitation.AltTextHu,
                invitation.AltTextEn,
                invitation.DisplayOrder))
            .ToList();
    }
}