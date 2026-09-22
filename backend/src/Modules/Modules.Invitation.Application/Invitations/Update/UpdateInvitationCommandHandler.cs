using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.Update;

internal sealed class UpdateInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork)
    : IRequestHandler<UpdateInvitationCommand>
{
    public async Task Handle(
        UpdateInvitationCommand request,
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

        invitation.UpdateDetails(
            request.TitleHu,
            request.TitleEn,
            request.Year,
            request.AltTextHu,
            request.AltTextEn,
            request.DisplayOrder);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}