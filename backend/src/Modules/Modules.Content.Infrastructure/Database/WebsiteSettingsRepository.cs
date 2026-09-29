using Microsoft.EntityFrameworkCore;
using Modules.Content.Application.Abstractions;

namespace Modules.Content.Infrastructure.Database;

internal sealed class WebsiteSettingsRepository(
    ContentDbContext dbContext)
    : IWebsiteSettingsRepository
{
    public void Add(
        Domain.WebsiteSettings websiteSettings)
    {
        dbContext.WebsiteSettings.Add(
            websiteSettings);
    }

    public async Task<Domain.WebsiteSettings?> GetAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.WebsiteSettings
            .AsNoTracking()
            .SingleOrDefaultAsync(
                settings =>
                    settings.Id ==
                    Domain.WebsiteSettings.SingletonId,
                cancellationToken);
    }

    public async Task<Domain.WebsiteSettings?>
        GetForUpdateAsync(
            CancellationToken cancellationToken = default)
    {
        return await dbContext.WebsiteSettings
            .SingleOrDefaultAsync(
                settings =>
                    settings.Id ==
                    Domain.WebsiteSettings.SingletonId,
                cancellationToken);
    }
}
