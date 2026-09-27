using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.Publish;

internal sealed class PublishInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork)
    : IRequestHandler<PublishInvitationCommand>
{
    public async Task Handle(
        PublishInvitationCommand request,
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

        invitation.Publish();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}