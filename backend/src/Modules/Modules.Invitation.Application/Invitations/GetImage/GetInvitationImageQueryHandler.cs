using MediatR;
using Modules.FileStorage.Contracts;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.GetImage;

internal sealed class GetInvitationImageQueryHandler(
    IInvitationRepository invitationRepository,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<
        GetInvitationImageQuery,
        InvitationImage?>
{
    public async Task<InvitationImage?> Handle(
        GetInvitationImageQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Invitation? invitation =
            await invitationRepository.GetPublishedByIdAsync(
                request.InvitationId,
                cancellationToken);

        if (invitation?.ImageId is null)
        {
            return null;
        }

        FileContentData? file =
            await fileStorageModule.GetFileContentAsync(
                invitation.ImageId.Value,
                cancellationToken);

        if (file is null ||
            !file.ContentType.StartsWith(
                "image/",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return new InvitationImage(
            file.Content,
            file.ContentType);
    }
}