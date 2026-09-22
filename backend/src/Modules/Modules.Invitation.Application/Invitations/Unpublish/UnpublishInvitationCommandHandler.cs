using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.Unpublish;

internal sealed class UnpublishInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork)
    : IRequestHandler<UnpublishInvitationCommand>
{
    public async Task Handle(
        UnpublishInvitationCommand request,
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

        invitation.Unpublish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}