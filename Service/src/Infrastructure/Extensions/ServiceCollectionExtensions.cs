using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext with conditional provider
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AccessDbContext>(options =>
        {
            // Use SQLite for testing environment, SQL Server for others
            if (connectionString?.Contains("Data Source") == true && !connectionString.Contains("Server="))
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        // Add repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

        return services;
    }
}
