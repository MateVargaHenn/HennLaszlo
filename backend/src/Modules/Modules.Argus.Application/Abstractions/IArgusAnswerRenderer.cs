namespace Modules.Argus.Application.Abstractions;

public interface IArgusAnswerRenderer
{
    Task<string> RenderAsync(
        string answer,
        CancellationToken cancellationToken = default);
}