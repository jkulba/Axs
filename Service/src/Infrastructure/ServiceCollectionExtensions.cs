using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // Add DbContext with enhanced logging for Development
        services.AddDbContext<AccessDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            
            // Enhanced logging for Development environment
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging(true);
                options.EnableDetailedErrors(true);
                options.LogTo(Console.WriteLine, new[]
                {
                    DbLoggerCategory.Database.Command.Name,
                    DbLoggerCategory.Database.Transaction.Name,
                    DbLoggerCategory.Database.Connection.Name
                }, LogLevel.Information);
            }
        });

        // Add repositories
        services.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
        services.AddScoped<IAccessGroupRepository, AccessGroupRepository>();

        return services;
    }
}