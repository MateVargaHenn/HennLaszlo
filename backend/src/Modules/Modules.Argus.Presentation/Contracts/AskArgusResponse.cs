namespace Modules.Argus.Presentation.Contracts;

public sealed record AskArgusResponse(
    string Answer,
    double Confidence,
    bool IsFallback,
    ArgusSourceResponse? Source);

public sealed record ArgusSourceResponse(
    string Title,
    string Path);