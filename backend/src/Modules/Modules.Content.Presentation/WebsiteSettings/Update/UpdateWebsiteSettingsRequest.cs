namespace Modules.Content.Presentation.WebsiteSettings.Update;

internal sealed record UpdateWebsiteSettingsRequest(
    string ArtistName,
    string ArtistSubtitle,
    string HeroDescription,
    string DefaultSeoTitle,
    string DefaultSeoDescription,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl);
