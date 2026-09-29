namespace Modules.Content.Application.WebsiteSettings.GetAdmin;

public sealed record AdminWebsiteSettingsDetails(
    Guid Id,
    string ArtistName,
    string ArtistSubtitle,
    string HeroDescription,
    string DefaultSeoTitle,
    string DefaultSeoDescription,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl,
    DateTime UpdatedAtUtc);
