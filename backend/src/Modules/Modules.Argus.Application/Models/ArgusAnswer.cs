namespace Modules.Argus.Application.Models;

public sealed record ArgusAnswer(
    string Answer,
    double Confidence,
    bool IsFallback,
    string? SourceTitle,
    string? SourcePath);