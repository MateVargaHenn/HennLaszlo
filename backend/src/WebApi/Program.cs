using WebApi.ExceptionHandling;
using Modules.Artwork.Infrastructure;
using Modules.Artwork.Application;
using Modules.Artwork.Presentation;
using Modules.FileStorage.Infrastructure;

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

builder.Services.AddArtworkApplication(mediatrLicenseKey);

builder.Services.AddArtworkInfrastructure(connectionString);

builder.Services.AddFileStorageInfrastructure(
    connectionString, 
    storageRootPath);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapArtworkEndpoints();
app.Run();

