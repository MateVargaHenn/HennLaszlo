using Modules.Artwork.Infrastructure;
using Modules.Artwork.Application;
using Modules.Artwork.Presentation;

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

builder.Services.AddArtworkApplication(mediatrLicenseKey);
builder.Services.AddArtworkInfrastructure(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapArtworkEndpoints();
app.Run();

