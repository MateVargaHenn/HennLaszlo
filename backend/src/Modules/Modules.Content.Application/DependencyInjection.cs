using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Content.Application;

public static class DependencyInjection
{
    public static IServiceCollection
        AddContentApplication(
            this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration
                .RegisterServicesFromAssembly(
                    typeof(DependencyInjection)
                        .Assembly));

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true);

        return services;
    }
}