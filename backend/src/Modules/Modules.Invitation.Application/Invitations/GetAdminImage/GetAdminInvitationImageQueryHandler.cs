using MediatR;
using Modules.FileStorage.Contracts;
using Modules.Invitation.Application.Abstractions;
using Modules.Invitation.Application.Invitations.GetImage;

namespace Modules.Invitation.Application.Invitations.GetAdminImage;

internal sealed class GetAdminInvitationImageQueryHandler(
    IInvitationRepository invitationRepository,
    IFileStorageModule fileStorageModule)
    : IRequestHandler<
        GetAdminInvitationImageQuery,
        InvitationImage?>
{
    public async Task<InvitationImage?> Handle(
        GetAdminInvitationImageQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Invitation? invitation =
            await invitationRepository.GetByIdAsync(
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