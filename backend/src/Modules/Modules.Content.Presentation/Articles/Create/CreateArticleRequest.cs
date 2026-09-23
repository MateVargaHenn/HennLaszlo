namespace Modules.Content.Presentation.Articles.Create;

internal sealed record CreateArticleRequest(
    string Slug,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn,
    int DisplayOrder);