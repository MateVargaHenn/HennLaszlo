namespace Modules.Invitation.Application.Invitations.GetAll;

public sealed record InvitationListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    int DisplayOrder);