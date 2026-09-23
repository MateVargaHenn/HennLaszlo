namespace Modules.Content.Application.ContentPages.GetPublishedByKey;

public sealed record PublishedContentPageDetails(
    string Key,
    string TitleHu,
    string? TitleEn,
    string ContentHu,
    string? ContentEn);