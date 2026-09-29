using MediatR;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Application.WebsiteSettings.Update;

internal sealed class UpdateWebsiteSettingsCommandHandler(
    IWebsiteSettingsRepository websiteSettingsRepository,
    IContentUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWebsiteSettingsCommand>
{
    public async Task Handle(
        UpdateWebsiteSettingsCommand request,
        CancellationToken cancellationToken)
    {
        Domain.WebsiteSettings? settings =
            await websiteSettingsRepository
                .GetForUpdateAsync(
                    cancellationToken);

        if (settings is null)
        {
            settings = Domain.WebsiteSettings.Create(
                request.ArtistName,
                request.ArtistSubtitle,
                request.HeroDescription,
                request.DefaultSeoTitle,
                request.DefaultSeoDescription,
                request.FacebookUrl,
                request.InstagramUrl,
                request.YoutubeUrl);

            websiteSettingsRepository.Add(settings);
        }
        else
        {
            settings.Update(
                request.ArtistName,
                request.ArtistSubtitle,
                request.HeroDescription,
                request.DefaultSeoTitle,
                request.DefaultSeoDescription,
                request.FacebookUrl,
                request.InstagramUrl,
                request.YoutubeUrl);
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}
