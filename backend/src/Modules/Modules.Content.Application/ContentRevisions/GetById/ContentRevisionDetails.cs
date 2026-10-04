namespace Modules.Content.Application
    .ContentRevisions.GetById;

public sealed record ContentRevisionDetails(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn,
    DateTime CreatedAtUtc);