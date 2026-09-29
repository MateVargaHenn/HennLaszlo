namespace Modules.Content.Application.WebsiteSettings.GetPublic;

public sealed record PublicWebsiteSettings(
    string ArtistName,
    string ArtistSubtitle,
    string HeroDescription,
    string DefaultSeoTitle,
    string DefaultSeoDescription,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl);
