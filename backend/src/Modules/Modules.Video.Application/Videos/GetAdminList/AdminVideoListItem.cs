namespace Modules.Video.Application.Videos.GetAdminList;

public sealed record AdminVideoListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string VideoUrl,
    bool IsPublished,
    int DisplayOrder,
    DateTime CreatedAtUtc);