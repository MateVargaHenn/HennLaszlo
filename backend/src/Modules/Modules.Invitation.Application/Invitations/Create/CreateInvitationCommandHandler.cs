using BuildingBlocks.Application.Ordering;
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

        invitation.SetExhibitionPeriod(
            request.ExhibitionStartsAt,
            request.ExhibitionEndsAt);

        invitation.SetLocation(
            request.LocationHu,
            request.LocationEn);

            IReadOnlyList<Domain.Invitation>
                orderedInvitations =
                    await invitationRepository
                        .GetOrderedForUpdateAsync(
                            cancellationToken);

            DisplayOrderManager.Place(
                orderedInvitations,
                invitation,
                request.DisplayOrder,
                static (item, position) =>
                    item.SetDisplayOrder(position));

            invitationRepository.Add(invitation);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return invitation.Id;
    }
}