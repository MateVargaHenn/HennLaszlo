using MediatR;

namespace Modules.Content.Application.Articles.Create;

public sealed record CreateArticleCommand(
    string Slug,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn)
    : IRequest<Guid>;