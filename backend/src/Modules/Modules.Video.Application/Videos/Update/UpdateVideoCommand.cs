using MediatR;

namespace Modules.Video.Application.Videos.Update;

public sealed record UpdateVideoCommand(
    Guid VideoId,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    int DisplayOrder)
    : IRequest;