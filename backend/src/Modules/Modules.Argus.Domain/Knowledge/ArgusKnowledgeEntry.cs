namespace Modules.Argus.Domain.Knowledge;

public sealed record ArgusKnowledgeEntry(
    string Id,
    string Answer,
    string? SourceTitle,
    string? SourcePath,
    IReadOnlyCollection<string> Questions,
    IReadOnlyCollection<string> Keywords)
{
    public IReadOnlyCollection<string> Concepts
    {
        get;
        init;
    } = [];

    public IReadOnlyCollection<string> RelatedEntityIds
    {
        get;
        init;
    } = [];

    public IReadOnlyCollection<string> NegativeExamples
    {
        get;
        init;
    } = [];
}