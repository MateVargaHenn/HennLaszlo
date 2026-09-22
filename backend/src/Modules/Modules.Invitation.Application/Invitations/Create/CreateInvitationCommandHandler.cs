using MediatR;
using Modules.Invitation.Application.Abstractions;

namespace Modules.Invitation.Application.Invitations.Create;

internal sealed class CreateInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IInvitationUnitOfWork unitOfWork)
    : IRequestHandler<CreateInvitationCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateInvitationCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Invitation invitation =
            Domain.Invitation.Create(
                request.TitleHu,
                request.TitleEn,
                request.Year,
                request.AltTextHu,
                request.AltTextEn,
                request.DisplayOrder);

        invitationRepository.Add(invitation);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return invitation.Id;
    }
}