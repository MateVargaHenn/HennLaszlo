namespace Modules.Content.Application.Articles.GetPublishedBySlug;

public sealed record PublishedArticleDetails(
    Guid Id,
    string Slug,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn);