using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.FileStorage.Infrastructure.Database;
using Modules.FileStorage.Application.Abstractions;
using Modules.FileStorage.Infrastructure.Storage;

namespace Modules.FileStorage.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorageInfrastructure(
        this IServiceCollection services,
        string connectionString,
    	string storageRootPath)
    {
        services.AddDbContext<FileStorageDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        "file_storage");
                });
        });
		
		services.AddSingleton<IFileContentStorage>(
			new LocalFileContentStorage(storageRootPath));

        return services;
    }
}