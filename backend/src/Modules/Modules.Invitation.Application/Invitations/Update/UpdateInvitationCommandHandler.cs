using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Ordering;
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
        IReadOnlyList<Domain.Invitation>
            orderedInvitations =
                await invitationRepository
                    .GetOrderedForUpdateAsync(
                        cancellationToken);

        Domain.Invitation? invitation =
            orderedInvitations.SingleOrDefault(
                item =>
                    item.Id == request.InvitationId);

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
            request.AltTextEn);

        invitation.SetExhibitionPeriod(
            request.ExhibitionStartsAt,
            request.ExhibitionEndsAt);

        invitation.SetLocation(
            request.LocationHu,
            request.LocationEn);

        DisplayOrderManager.Place(
            orderedInvitations,
            invitation,
            request.DisplayOrder,
            static (item, position) =>
                item.SetDisplayOrder(position));

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}