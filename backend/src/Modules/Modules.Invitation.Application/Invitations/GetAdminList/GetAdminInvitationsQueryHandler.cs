using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.GetAdminList;

internal sealed class GetAdminInvitationsQueryHandler(
    IInvitationRepository invitationRepository)
    : IRequestHandler<
        GetAdminInvitationsQuery,
        IReadOnlyList<AdminInvitationListItem>>
{
    public async Task<
        IReadOnlyList<AdminInvitationListItem>> Handle(
            GetAdminInvitationsQuery request,
            CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Invitation> invitations =
            await invitationRepository.GetAllAsync(
                cancellationToken);

        return invitations
            .Select(invitation =>
                new AdminInvitationListItem(
                    invitation.Id,
                    invitation.TitleHu,
                    invitation.TitleEn,
                    invitation.Year,
                    invitation.ImageId.HasValue,
                    invitation.IsPublished,
                    invitation.DisplayOrder,
                    invitation.CreatedAtUtc))
            .ToList();
    }
}