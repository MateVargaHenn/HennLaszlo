using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Invitation.Application.Abstractions;
using Modules.Invitation.Infrastructure.Database;

namespace Modules.Invitation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInvitationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException(
                "A Database connection string nincs beállítva.");

        services.AddDbContext<InvitationDbContext>(
            options => options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(
                        "__EFMigrationsHistory",
                        "invitation")));

        services.AddScoped<IInvitationUnitOfWork>(
            serviceProvider =>
                serviceProvider.GetRequiredService<
                    InvitationDbContext>());

		services.AddScoped<
			IInvitationRepository,
			InvitationRepository>();

        return services;
    }
}