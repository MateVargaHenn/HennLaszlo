using WebApi.ExceptionHandling;
using Modules.Artwork.Infrastructure;
using Modules.Artwork.Application;
using Modules.Artwork.Presentation;
using Modules.FileStorage.Infrastructure;
using Modules.FileStorage.Application;
using Modules.FileStorage.Presentation;
using BuildingBlocks.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

builder.Services.AddArtworkInfrastructure(connectionString);

builder.Services.AddFileStorageInfrastructure(
    connectionString, 
    storageRootPath);

builder.Services.AddFileStorageApplication(
    mediatrLicenseKey);

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

app.MapArtworkEndpoints();
app.MapFileStorageEndpoints();

app.Run();

