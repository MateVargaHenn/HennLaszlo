namespace Modules.Video.Presentation.Videos.Create;

public sealed record CreateVideoRequest(
    string TitleHu,
    string? TitleEn,
    int? Year,
    string? DescriptionHu,
    string? DescriptionEn,
    string VideoUrl,
    int DisplayOrder = 0);