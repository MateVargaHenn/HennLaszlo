using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Content.Application.Abstractions;
using Modules.Content.Infrastructure.Database;

namespace Modules.Content.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection
        AddContentInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString(
                "Database")
            ?? throw new InvalidOperationException(
                "A Database connection string hiányzik.");

        services.AddDbContext<ContentDbContext>(
            options =>
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                        npgsqlOptions
                            .MigrationsHistoryTable(
                                "__EFMigrationsHistory",
                                "content")));

        services.AddScoped<
            IContentPageRepository,
            ContentPageRepository>();

        services.AddScoped<IContentUnitOfWork>(
            serviceProvider =>
                serviceProvider
                    .GetRequiredService<
                        ContentDbContext>());

        services.AddScoped<
            IArticleRepository,
            ArticleRepository>();

        return services;
    }
}