using Microsoft.AspNetCore.HttpOverrides;
using WebApi.ExceptionHandling;
using Modules.Artwork.Infrastructure;
using Modules.Artwork.Application;
using Modules.Artwork.Presentation;
using Modules.FileStorage.Infrastructure;
using Modules.FileStorage.Application;
using Modules.FileStorage.Presentation;
using Modules.Invitation.Infrastructure;
using Modules.Invitation.Application;
using Modules.Invitation.Presentation;
using Modules.Content.Infrastructure;
using Modules.Content.Application;
using Modules.Content.Presentation;
using BuildingBlocks.Application;
using WebApi.Authentication;

if (args.Contains(
        "--hash-admin-password",
        StringComparer.Ordinal))
{
    AdminPasswordHashGenerator.Run();
    return;
}

var builder =
    WebApplication.CreateBuilder(args);

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

string mediatrLicenseKey =
    builder.Configuration["MEDIATR_LICENSE_KEY"]
    ?? throw new InvalidOperationException(
        "A MediatR licenckulcs nincs beállítva.");

string storageRootPath =
    builder.Configuration["FileStorage:RootPath"]
    ?? Path.Combine(
        builder.Environment.ContentRootPath,
        "storage");
        
builder.Services.AddApplicationBuildingBlocks();

builder.Services.AddArtworkApplication(mediatrLicenseKey);
builder.Services.AddInvitationApplication();

builder.Services.AddArtworkInfrastructure(connectionString);
builder.Services.AddInvitationInfrastructure(builder.Configuration);

builder.Services.AddFileStorageInfrastructure(
    connectionString, 
    storageRootPath);

builder.Services.AddFileStorageApplication(
    mediatrLicenseKey);

builder.Services.AddContentInfrastructure(
    builder.Configuration);
builder.Services.AddContentApplication();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();

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

var app = builder.Build();

app.UseForwardedHeaders();
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

app.MapPublicArtworkEndpoints();
app.MapPublicInvitationEndpoints();
app.MapPublicContentEndpoints();

RouteGroupBuilder adminEndpoints =
    app.MapGroup(string.Empty)
        .RequireAuthorization(
            AuthenticationExtensions
                .AdminAuthorizationPolicy)
        .ValidateAntiforgery();

adminEndpoints.MapAdminArtworkEndpoints();
adminEndpoints.MapAdminInvitationEndpoints();
adminEndpoints.MapAdminFileStorageEndpoints();
adminEndpoints.MapAdminContentEndpoints();

app.Run();

