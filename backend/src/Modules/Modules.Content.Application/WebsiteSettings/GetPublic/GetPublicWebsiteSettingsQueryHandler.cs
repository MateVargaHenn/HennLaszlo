using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.WebsiteSettings.GetPublic;

internal sealed class GetPublicWebsiteSettingsQueryHandler(
    IWebsiteSettingsRepository websiteSettingsRepository)
    : IRequestHandler<
        GetPublicWebsiteSettingsQuery,
        PublicWebsiteSettings>
{
    public async Task<PublicWebsiteSettings> Handle(
        GetPublicWebsiteSettingsQuery request,
        CancellationToken cancellationToken)
    {
        Domain.WebsiteSettings? settings =
            await websiteSettingsRepository.GetAsync(
                cancellationToken);

        if (settings is null)
        {
            throw new NotFoundException(
                "A weboldal beállításai nem találhatók.");
        }

        return new PublicWebsiteSettings(
            settings.ArtistName,
            settings.ArtistSubtitle,
            settings.HeroDescription,
            settings.DefaultSeoTitle,
            settings.DefaultSeoDescription,
            settings.FacebookUrl,
            settings.InstagramUrl,
            settings.YoutubeUrl);
    }
}
