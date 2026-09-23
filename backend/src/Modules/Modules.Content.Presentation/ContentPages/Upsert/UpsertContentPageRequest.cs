namespace Modules.Content.Presentation.ContentPages.Upsert;

internal sealed record UpsertContentPageRequest(
    string TitleHu,
    string? TitleEn,
    string ContentHu,
    string? ContentEn);