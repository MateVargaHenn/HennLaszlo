namespace Modules.Content.Application.ContentPages.GetAdminList;

public sealed record AdminContentPageListItem(
    Guid Id,
    string Key,
    string TitleHu,
    string? TitleEn,
    bool IsPublished,
    DateTime UpdatedAtUtc);