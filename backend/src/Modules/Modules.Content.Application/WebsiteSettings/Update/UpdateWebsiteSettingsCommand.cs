using MediatR;

namespace Modules.Content.Application.WebsiteSettings.Update;

public sealed record UpdateWebsiteSettingsCommand(
    string ArtistName,
    string ArtistSubtitle,
    string HeroDescription,
    string DefaultSeoTitle,
    string DefaultSeoDescription,
    string? FacebookUrl,
    string? InstagramUrl,
    string? YoutubeUrl,
    string? EmailAddress)
    : IRequest;
