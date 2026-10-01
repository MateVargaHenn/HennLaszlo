using BuildingBlocks.Application.Exceptions;
using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.WebsiteSettings.GetAdmin;

internal sealed class GetAdminWebsiteSettingsQueryHandler(
    IWebsiteSettingsRepository websiteSettingsRepository)
    : IRequestHandler<
        GetAdminWebsiteSettingsQuery,
        AdminWebsiteSettingsDetails>
{
    public async Task<AdminWebsiteSettingsDetails> Handle(
        GetAdminWebsiteSettingsQuery request,
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

        return new AdminWebsiteSettingsDetails(
            settings.Id,
            settings.ArtistName,
            settings.ArtistSubtitle,
            settings.HeroDescription,
            settings.DefaultSeoTitle,
            settings.DefaultSeoDescription,
            settings.FacebookUrl,
            settings.InstagramUrl,
            settings.YoutubeUrl,
            settings.EmailAddress,
            settings.UpdatedAtUtc);
    }
}
