

using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Modules.Artwork.Application.Abstractions.Behaviors;

namespace Modules.Artwork.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddArtworkApplication(
        this IServiceCollection services,
        string mediatrLicenseKey)
    {
        services.AddValidatorsFromAssembly(
                    typeof(DependencyInjection).Assembly,
                    includeInternalTypes: true);

        ArgumentException.ThrowIfNullOrWhiteSpace(mediatrLicenseKey);
        
        services.AddMediatR(configuration =>
        {
            configuration.LicenseKey = mediatrLicenseKey;

            configuration.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);

                configuration.AddOpenBehavior(
                    typeof(ValidationBehavior<,>));
        });

        return services;
    }
}