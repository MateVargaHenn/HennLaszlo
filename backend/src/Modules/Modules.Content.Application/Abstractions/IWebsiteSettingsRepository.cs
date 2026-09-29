namespace Modules.Content.Application.Abstractions;

public interface IWebsiteSettingsRepository
{
    void Add(
        Domain.WebsiteSettings websiteSettings);

    Task<Domain.WebsiteSettings?> GetAsync(
        CancellationToken cancellationToken = default);

    Task<Domain.WebsiteSettings?> GetForUpdateAsync(
        CancellationToken cancellationToken = default);
}
