using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Domain.Repositores;
using FIAPCloudGames.Infrastructure.Context;
using FIAPCloudGames.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAPCloudGames.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGameRepository, GameRepository>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        // PostgreSQL Configuration
        services.AddDbContextPool<FCGContext>(options =>
        {
            options.UseNpgsql(connectionString,
            op =>
            {
                op.CommandTimeout(300);
                op.MaxBatchSize(1);
            });
            
            // Configurar warnings para suprimir o aviso de mudanças pendentes no modelo
            options.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        })
        .AddLogging();

        return services;
    }
}
