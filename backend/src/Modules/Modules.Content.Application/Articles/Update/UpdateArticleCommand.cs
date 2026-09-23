using MediatR;

namespace Modules.Content.Application.Articles.Update;

public sealed record UpdateArticleCommand(
    Guid ArticleId,
    string TitleHu,
    string? TitleEn,
    string? SummaryHu,
    string? SummaryEn,
    string ContentHu,
    string? ContentEn,
    int DisplayOrder)
    : IRequest;