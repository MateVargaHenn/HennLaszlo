namespace Modules.Content.Application.Articles.GetPublishedList;

public sealed record PublishedArticleListItem(
    Guid Id,
    string Slug,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn);