using MediatR;

namespace Modules.Invitation.Application.Invitations.Update;

public sealed record UpdateInvitationCommand(
    Guid InvitationId,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    int DisplayOrder)
    : IRequest;