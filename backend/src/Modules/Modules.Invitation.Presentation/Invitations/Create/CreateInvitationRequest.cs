namespace Modules.Invitation.Presentation.Invitations.Create;

public sealed record CreateInvitationRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    int DisplayOrder = 0,
    DateTimeOffset? ExhibitionStartsAt = null,
    DateTimeOffset? ExhibitionEndsAt = null,
    string? LocationHu = null,
    string? LocationEn = null);