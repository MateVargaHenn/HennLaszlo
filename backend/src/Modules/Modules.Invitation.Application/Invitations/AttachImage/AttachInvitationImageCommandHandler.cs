using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Modules.FileStorage.Contracts;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.AttachImage;

internal sealed class AttachInvitationImageCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<AttachInvitationImageCommand>
{
    public async Task Handle(
        AttachInvitationImageCommand request,
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

        FileMetadata? file =
            await fileStorageModule.GetFileMetadataAsync(
                request.FileId,
                cancellationToken);

        if (file is null)
        {
            throw new NotFoundException(
                "A feltöltött fájl nem található.");
        }

        if (!file.ContentType.StartsWith(
                "image/",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException(
                new[]
                {
                    new ValidationFailure(
                        nameof(request.FileId),
                        "A kiválasztott fájl nem kép."),
                });
        }

        Guid? previousImageId = invitation.ImageId;

        invitation.SetImage(request.FileId);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        if (previousImageId.HasValue &&
            previousImageId.Value != request.FileId)
        {
            await fileStorageModule.DeleteFileAsync(
                previousImageId.Value,
                cancellationToken);
        }
    }
}