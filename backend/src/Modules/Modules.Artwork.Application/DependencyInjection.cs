

using Microsoft.Extensions.DependencyInjection;

namespace Modules.Artwork.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddArtworkApplication(
        this IServiceCollection services,
        string mediatrLicenseKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mediatrLicenseKey);
        
        services.AddMediatR(configuration =>
        {
            configuration.LicenseKey = mediatrLicenseKey;

            configuration.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}