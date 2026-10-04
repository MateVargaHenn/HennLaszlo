using MediatR;

namespace Modules.Content.Application.ContentPages.Upsert;

public sealed record UpsertContentPageCommand(
    string Key,
    string TitleHu,
    string? TitleEn,
    string ContentHu,
    string? ContentEn,
    Guid? ExpectedVersion)
    : IRequest<Guid>;