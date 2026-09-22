using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.GetAdminById;

internal sealed class GetAdminInvitationByIdQueryHandler(
    IInvitationRepository invitationRepository)
    : IRequestHandler<
        GetAdminInvitationByIdQuery,
        AdminInvitationDetails>
{
    public async Task<AdminInvitationDetails> Handle(
        GetAdminInvitationByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Invitation? invitation =
            await invitationRepository.GetByIdAsync(
                request.InvitationId,
                cancellationToken);

        if (invitation is null)
        {
            throw new NotFoundException(
                "A meghívó nem található.");
        }

        return new AdminInvitationDetails(
            invitation.Id,
            invitation.TitleHu,
            invitation.TitleEn,
            invitation.Year,
            invitation.AltTextHu,
            invitation.AltTextEn,
            invitation.ImageId.HasValue,
            invitation.IsPublished,
            invitation.DisplayOrder);
    }
}