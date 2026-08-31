using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.FileStorage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorageApplication(
        this IServiceCollection services,
        string mediatrLicenseKey)
    {
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true);

        services.AddMediatR(configuration =>
        {
            configuration.LicenseKey = mediatrLicenseKey;

            configuration.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}