

using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Modules.Artwork.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddArtworkApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
                    typeof(DependencyInjection).Assembly,
                    includeInternalTypes: true);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);

        });

        return services;
    }
}
