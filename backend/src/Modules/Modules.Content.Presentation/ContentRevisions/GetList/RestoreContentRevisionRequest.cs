namespace Modules.Content.Presentation
    .ContentRevisions.GetList;

internal sealed record RestoreContentRevisionRequest(
    Guid ExpectedVersion);
