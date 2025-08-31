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
        // Add DbContext with provider based on appsettings
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var provider = configuration["DatabaseProvider"] ?? "SqlServer";

        services.AddDbContext<AccessDbContext>(options =>
        {
            if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
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
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
