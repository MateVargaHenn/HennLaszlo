using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Invitation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInvitationApplication(
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