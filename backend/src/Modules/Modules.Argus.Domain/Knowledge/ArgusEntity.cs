namespace Modules.Argus.Domain.Knowledge;

public sealed record ArgusEntity(
    string Id,
    string Type,
    string CanonicalName,
    IReadOnlyCollection<string> Aliases);