using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Video.Application.Abstractions;
using Modules.Video.Infrastructure.Database;

namespace Modules.Video.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVideoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException(
                "A Database connection string nincs beállítva.");

        services.AddDbContext<VideoDbContext>(
            options => options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        "video")));

        services.AddScoped<IVideoUnitOfWork>(
            serviceProvider =>
                serviceProvider.GetRequiredService<
                    VideoDbContext>());

		services.AddScoped<
			IVideoRepository,
			VideoRepository>();

        return services;
    }
}