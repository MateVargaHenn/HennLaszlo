using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Video.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVideoApplication(
        this IServiceCollection services)
    {
        Assembly assembly =
            typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(
                assembly));

        services.AddValidatorsFromAssembly(
            assembly,
            includeInternalTypes: true);

        return services;
    }
}