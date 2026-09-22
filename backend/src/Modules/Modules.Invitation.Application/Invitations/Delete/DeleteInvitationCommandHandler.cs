using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.FileStorage.Contracts;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.Delete;

internal sealed class DeleteInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<DeleteInvitationCommand>
{
    public async Task Handle(
        DeleteInvitationCommand request,
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

        if (invitation.IsPublished)
        {
            throw new InvalidOperationException(
                "Publikált meghívó nem törölhető.");
        }

        Guid? imageId = invitation.ImageId;

        invitationRepository.Remove(invitation);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        if (imageId.HasValue)
        {
            await fileStorageModule.DeleteFileAsync(
                imageId.Value,
                cancellationToken);
        }
    }
}