namespace Modules.Invitation.Application.Invitations.GetAdminById;

public sealed record AdminInvitationDetails(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? AltTextHu,
    string? AltTextEn,
    bool HasImage,
    bool IsPublished,
    int DisplayOrder,
    DateTimeOffset? ExhibitionStartsAt,
    DateTimeOffset? ExhibitionEndsAt,
    string? LocationHu,
    string? LocationEn);