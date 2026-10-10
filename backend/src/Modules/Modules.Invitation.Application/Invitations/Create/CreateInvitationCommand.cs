using MediatR;

namespace Modules.Invitation.Application.Invitations.Create;

public sealed record CreateInvitationCommand(
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
    : IRequest<Guid>;