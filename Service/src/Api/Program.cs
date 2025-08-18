using System.Text.Json;
using System.Text.RegularExpressions;

using Serilog;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using FluentValidation;

using Application;
using Infrastructure;
using Infrastructure.Extensions;
using Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Extract port number from configuration
string portNumber = "unknown";
var urls = builder.Configuration["urls"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

if (!string.IsNullOrEmpty(urls))
{
    // Extract port from URLs (e.g., "http://*:3010" or "https://localhost:3011")
    var match = Regex.Match(urls, ":([0-9]+)");
    if (match.Success)
    {
        portNumber = match.Groups[1].Value;
    }
}

// Add configuration sources
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// Configure Serilog with port enrichment
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("Port", portNumber));


// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Axs Control Service API",
        Description = "Access Control RESTful Services Documentation."
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Axs Control Service API v2");
    });
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapAccessRequestEndpoints();
app.MapActivityEndpoints();
app.MapAuthorizationEndpoints();

app.MapGet("/api/version", async () =>
{
    var filePath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "version.json");
    if (File.Exists(filePath))
    {
        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            return Results.Content(jsonContent, "application/json");
        }
        catch (IOException ex)
        {
            Log.Error(ex, "Error reading version.json file");
            return Results.Problem("Error reading version.json file", statusCode: 500);
        }


    }
    return Results.NotFound("version.json file not found");
})
.WithName("GetVersion")
.WithSummary("Get application version")
.WithDescription("Returns the current version of the application.")
.WithTags("Health")
.WithOpenApi();

app.Run();

// Make the implicit Program class public so it can be used by WebApplicationFactory in tests
public partial class Program { }
