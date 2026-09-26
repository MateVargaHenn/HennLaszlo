namespace Modules.Argus.Domain.Knowledge;

public sealed record ArgusSubject(
    string Id,
    string CanonicalName,
    IReadOnlyCollection<string> Aliases)
{
    public IReadOnlyCollection<string> References
    {
        get;
        init;
    } = [];

    public IReadOnlyCollection<string> Roles
    {
        get;
        init;
    } = [];
}