namespace Modules.Video.Application.Videos;

internal static class VideoUrlValidator
{
    internal static bool IsValid(
        string? videoUrl)
    {
        return
            Uri.TryCreate(
                videoUrl,
                UriKind.Absolute,
                out Uri? uri) &&
            uri.Scheme == Uri.UriSchemeHttps;
    }
}