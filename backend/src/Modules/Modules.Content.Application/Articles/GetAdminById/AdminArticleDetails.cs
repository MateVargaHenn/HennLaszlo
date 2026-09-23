namespace Modules.Content.Application.Articles.GetAdminById;

public sealed record AdminArticleDetails(
    Guid Id,
    string Slug,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn,
    bool IsPublished,
    int DisplayOrder,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);