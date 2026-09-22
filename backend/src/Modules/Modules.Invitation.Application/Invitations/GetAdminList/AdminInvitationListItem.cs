namespace Modules.Invitation.Application.Invitations.GetAdminList;

public sealed record AdminInvitationListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    bool HasImage,
    bool IsPublished,
    int DisplayOrder,
    DateTime CreatedAtUtc);