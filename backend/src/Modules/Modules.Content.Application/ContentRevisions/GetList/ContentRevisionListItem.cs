namespace Modules.Content.Application
    .ContentRevisions.GetList;

public sealed record ContentRevisionListItem(
    Guid Id,
    string TitleHu,
    DateTime CreatedAtUtc);