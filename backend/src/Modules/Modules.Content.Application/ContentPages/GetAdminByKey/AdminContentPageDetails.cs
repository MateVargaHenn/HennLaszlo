namespace Modules.Content.Application.ContentPages.GetAdminByKey;

public sealed record AdminContentPageDetails(
    Guid Id,
    string Key,
    string TitleHu,
    string? TitleEn,
    string ContentHu,
    string? ContentEn,
    bool IsPublished,
    DateTime UpdatedAtUtc);