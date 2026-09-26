namespace Modules.Argus.Domain.Knowledge;

public sealed record ArgusKnowledgeDocument(
    ArgusSubject Subject,
    IReadOnlyCollection<ArgusKnowledgeEntry> Entries)
{
    public IReadOnlyCollection<ArgusEntity> Entities
    {
        get;
        init;
    } = [];
}