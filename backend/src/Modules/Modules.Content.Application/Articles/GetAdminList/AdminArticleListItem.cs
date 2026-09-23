namespace Modules.Content.Application.Articles.GetAdminList;

public sealed record AdminArticleListItem(
    Guid Id,
    string Slug,
    string TitleHu,
    string? TitleEn,
    bool IsPublished,
    int DisplayOrder,
    DateTime UpdatedAtUtc);