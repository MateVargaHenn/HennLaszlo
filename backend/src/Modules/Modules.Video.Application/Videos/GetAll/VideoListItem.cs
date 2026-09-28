namespace Modules.Video.Application.Videos.GetAll;

public sealed record VideoListItem(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    int DisplayOrder);