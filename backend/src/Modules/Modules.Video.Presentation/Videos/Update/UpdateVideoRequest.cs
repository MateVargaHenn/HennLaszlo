namespace Modules.Video.Presentation.Videos.Update;

public sealed record UpdateVideoRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    int DisplayOrder);