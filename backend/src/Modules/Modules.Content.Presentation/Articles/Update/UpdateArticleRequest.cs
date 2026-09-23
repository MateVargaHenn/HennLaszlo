namespace Modules.Content.Presentation.Articles.Update;

internal sealed record UpdateArticleRequest(
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn,
    int DisplayOrder);