namespace Modules.Video.Application.Videos.GetAdminById;

public sealed record AdminVideoDetails(
    Guid Id,
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    bool IsPublished,
    int DisplayOrder);