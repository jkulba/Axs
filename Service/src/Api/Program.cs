using System.Text.Json;
using System.Text.RegularExpressions;
// using Api.Endpoints;
// using Api.Models;
using Application;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;


// using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);

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
// builder.Services.AddApplicationServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiDocumentTransformer>();
});

builder.Services.AddSingleton<OpenApiDocumentTransformer>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost:") ||
                origin.StartsWith("https://localhost:"))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Add middleware to enrich log context with port for each request
app.Use(async (context, next) =>
{
    using (LogContext.PushProperty("Port", portNumber))
    {
        await next();
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Volcano API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Volcano API Documentation";
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowBlazorClient");
app.UseDefaultFiles();
app.UseStaticFiles();

// app.MapTelemetryEventEndpoints();
// app.MapOpenTelemetryEventEndpoints();

// app.MapGet("/api/health", async (HttpContext context) =>
// {
//     var healthCheckService = context.RequestServices.GetRequiredService<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService>();
//     var report = await healthCheckService.CheckHealthAsync();

//     var result = new HealthCheckResponse
//     {
//         Status = report.Status.ToString(),
//         Checks = report.Entries.Select(e => new HealthCheckItem
//         {
//             Name = e.Key,
//             Status = e.Value.Status.ToString(),
//             Description = e.Value.Description,
//             Duration = e.Value.Duration.TotalMilliseconds
//         }),
//         TotalDuration = report.TotalDuration.TotalMilliseconds
//     };

//     var statusCode = report.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy
//         ? StatusCodes.Status200OK
//         : StatusCodes.Status503ServiceUnavailable;

//     return Results.Json(result, statusCode: statusCode);
// })
// .WithName("GetHealthStatus")
// .WithSummary("Get application health status")
// .WithDescription("Returns the health status of the application including database connectivity checks.")
// .WithTags("Health")
// .WithOpenApi();

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

public class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Volcano API",
            Version = "v1",
            Description = "API for Volcano Telemetry Services",
            Contact = new OpenApiContact
            {
                Name = "Hallcrest Engineering Team",
                Email = "jkulba@gmail.com"
            }
        };

        document.Servers = new List<OpenApiServer>
        {
            new() { Url = "http://localhost:5001", Description = "Http Development server" },
            new() { Url = "https://localhost:7001", Description = "Https Development server" },
            new() { Url = "https://api.volcano.com", Description = "Https Production server" }
        };

        return Task.CompletedTask;
    }
}