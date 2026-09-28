using MediatR;

namespace Modules.Video.Application.Videos.Create;

public sealed record CreateVideoCommand(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    int DisplayOrder)
    : IRequest<Guid>;