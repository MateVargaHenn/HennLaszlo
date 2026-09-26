using Modules.Argus.Application.Models;

namespace Modules.Argus.Application.Abstractions;

public interface IArgusAnswerService
{
    Task<ArgusAnswer> AskAsync(
        string question,
        CancellationToken cancellationToken = default);
}