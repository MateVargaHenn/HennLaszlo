namespace Modules.Invitation.Presentation.Invitations.Update;

public sealed record UpdateInvitationRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    int DisplayOrder);