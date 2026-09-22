using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Artwork.Infrastructure.Database;
using Modules.Artwork.Application.Abstractions;
using Modules.Artwork.Infrastructure.Repositories;

namespace Modules.Artwork.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddArtworkInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ArtworkDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        "artwork");
                });
        });

		services.AddScoped<IArtworkRepository, ArtworkRepository>();

		services.AddScoped<IArtworkUnitOfWork>(
			serviceProvider =>
				serviceProvider.GetRequiredService<ArtworkDbContext>());

        return services;
    }
}