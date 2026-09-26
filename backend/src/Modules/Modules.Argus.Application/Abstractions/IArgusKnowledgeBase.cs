using Modules.Argus.Domain.Knowledge;

namespace Modules.Argus.Application.Abstractions;

public interface IArgusKnowledgeBase
{
    Task<ArgusKnowledgeDocument> GetAsync(
        CancellationToken cancellationToken = default);
}