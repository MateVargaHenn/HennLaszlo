using Microsoft.Extensions.DependencyInjection;
using Modules.Argus.Application.Abstractions;
using Modules.Argus.Infrastructure.Knowledge;

namespace Modules.Argus.Infrastructure;

public static class ArgusInfrastructureExtensions
{
    public static IServiceCollection
        AddArgusInfrastructure(
            this IServiceCollection services)
    {
        services.AddSingleton<
            IArgusKnowledgeBase,
            JsonArgusKnowledgeBase>();

        return services;
    }
}