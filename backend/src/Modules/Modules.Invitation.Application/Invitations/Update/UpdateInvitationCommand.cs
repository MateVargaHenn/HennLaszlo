using MediatR;

namespace Modules.Invitation.Application.Invitations.Update;

public sealed record UpdateInvitationCommand(
    Guid InvitationId,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    int DisplayOrder,
    DateTimeOffset? ExhibitionStartsAt = null,
    DateTimeOffset? ExhibitionEndsAt = null,
    string? LocationHu = null,
    string? LocationEn = null)
    : IRequest;