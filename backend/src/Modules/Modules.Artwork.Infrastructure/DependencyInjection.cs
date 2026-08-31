using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Artwork.Infrastructure.Database;

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

        return services;
    }
}