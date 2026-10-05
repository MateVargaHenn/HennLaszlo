using System.Net;
using System.Net.Sockets;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Modules.Argus.Application.Abstractions;
using Modules.Argus.Infrastructure;
using Modules.Argus.Presentation;
using Modules.Artwork.Application;
using Modules.Artwork.Infrastructure;
using Modules.Artwork.Infrastructure.Database;
using Modules.Artwork.Presentation;
using Modules.Content.Application;
using Modules.Content.Infrastructure;
using Modules.Content.Infrastructure.Database;
using Modules.Content.Presentation;
using Modules.FileStorage.Application;
using Modules.FileStorage.Infrastructure;
using Modules.FileStorage.Infrastructure.Database;
using Modules.FileStorage.Presentation;
using Modules.Invitation.Application;
using Modules.Invitation.Infrastructure;
using Modules.Invitation.Infrastructure.Database;
using Modules.Invitation.Presentation;
using Modules.Video.Application;
using Modules.Video.Infrastructure;
using Modules.Video.Infrastructure.Database;
using Modules.Video.Presentation;
using Serilog;
using WebApi.Argus;
using WebApi.Authentication;
using WebApi.ExceptionHandling;

if (args.Contains(
        "--hash-admin-password",
        StringComparer.Ordinal))
{
    AdminPasswordHashGenerator.Run();
    return;
}

var builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) =>
{
    logger.MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console();

    string? seqUrl = context.Configuration["Seq:ServerUrl"];
    if (!string.IsNullOrWhiteSpace(seqUrl))
    {
        logger.WriteTo.Seq(seqUrl);
    }
});

builder.Services.Configure<ForwardedHeadersOptions>(
    options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto;

        options.ForwardLimit = 1;
    });

    builder.Services
    .AddOptions<AdminAuthenticationOptions>()
    .BindConfiguration(
        AdminAuthenticationOptions.SectionName)
    .Validate(
        options =>
            !string.IsNullOrWhiteSpace(
                options.Username),
        "Az admin felhasználónév nincs beállítva.")
    .Validate(
        options =>
            !string.IsNullOrWhiteSpace(
                options.PasswordHash),
        "Az admin jelszó hash nincs beállítva.")
    .ValidateOnStart();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAdminAuthentication(
    builder.Environment);

string connectionString =
    builder.Configuration.GetConnectionString("Database")
    ?? throw new InvalidOperationException(
        "A 'Database' connection string nincs beállítva.");

string storageRootPath =
    builder.Configuration["FileStorage:RootPath"]
    ?? Path.Combine(
        builder.Environment.ContentRootPath,
        "storage");
        
builder.Services.AddApplicationBuildingBlocks();

builder.Services.AddArtworkApplication();
builder.Services.AddInvitationApplication();
builder.Services.AddVideoApplication();

builder.Services.AddArtworkInfrastructure(connectionString);
builder.Services.AddInvitationInfrastructure(builder.Configuration);
builder.Services.AddVideoInfrastructure(builder.Configuration);

builder.Services.AddFileStorageInfrastructure(
    connectionString, 
    storageRootPath);

builder.Services.AddFileStorageApplication();

builder.Services.AddContentInfrastructure(
    builder.Configuration);
builder.Services.AddContentApplication();

builder.Services
    .AddArgusInfrastructure()
    .AddArgusPresentation();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();

const string frontendCorsPolicy = "Frontend";

string[] allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()
    ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        frontendCorsPolicy,
        policy =>
        {
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddScoped<
    IArgusAnswerRenderer,
    ArgusAnswerRenderer>();

var app = builder.Build();

// One API instance owns migrations; Compose starts it only after PostgreSQL is healthy.
using (IServiceScope scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<ArtworkDbContext>()
        .Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<FileStorageDbContext>()
        .Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<InvitationDbContext>()
        .Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<VideoDbContext>()
        .Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ContentDbContext>()
        .Database.MigrateAsync();
}

app.UseForwardedHeaders();
// Docker assigns the gateway a new address on recreation. Trust its current
// service address only, rather than trusting forwarded headers from all peers.
app.Use(async (context, next) =>
{
    if (context.Request.Headers["X-Forwarded-Proto"] == "https" &&
        context.Connection.RemoteIpAddress is IPAddress remoteAddress)
    {
        if (remoteAddress.IsIPv4MappedToIPv6)
        {
            remoteAddress = remoteAddress.MapToIPv4();
        }

        try
        {
            IPAddress[] gatewayAddresses =
                await Dns.GetHostAddressesAsync(
                    "gateway",
                    context.RequestAborted);

            if (gatewayAddresses.Contains(remoteAddress))
            {
                context.Request.Scheme = "https";
            }
        }
        catch (SocketException)
        {
            // Keep the connection's actual HTTP scheme if the gateway is unavailable.
        }
    }

    await next(context);
});
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(frontendCorsPolicy);

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapAdminAuthenticationEndpoints();
app.MapGet("/health/ready", () => Results.Ok(new { status = "ready" }));

app.MapPublicArtworkEndpoints();
app.MapPublicInvitationEndpoints();
app.MapPublicVideoEndpoints();
app.MapPublicContentEndpoints();
app.MapArgusEndpoints();

RouteGroupBuilder adminEndpoints =
    app.MapGroup(string.Empty)
        .RequireAuthorization(
            AuthenticationExtensions
                .AdminAuthorizationPolicy)
        .ValidateAntiforgery();

adminEndpoints.MapAdminArtworkEndpoints();
adminEndpoints.MapAdminInvitationEndpoints();
adminEndpoints.MapAdminVideoEndpoints();
adminEndpoints.MapAdminFileStorageEndpoints();
adminEndpoints.MapAdminContentEndpoints();

app.Run();
