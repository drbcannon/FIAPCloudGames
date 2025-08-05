using FIAPCloudGames.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.WebAPI.Services;

public class DatabaseMigrationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(IServiceProvider serviceProvider, ILogger<DatabaseMigrationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FCGContext>();

        try
        {
            _logger.LogInformation("Iniciando migração automática do banco de dados...");

            // Verifica se o banco de dados existe
            var canConnect = await context.Database.CanConnectAsync(cancellationToken);
            _logger.LogInformation("Conexão com banco de dados: {CanConnect}", canConnect ? "Sucesso" : "Falha");

            // Verifica migrações pendentes
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
            var pendingCount = pendingMigrations.Count();
            
            if (pendingCount > 0)
            {
                _logger.LogInformation("Encontradas {Count} migrações pendentes", pendingCount);
                foreach (var migration in pendingMigrations)
                {
                    _logger.LogInformation("Migração pendente: {Migration}", migration);
                }
            }
            else
            {
                _logger.LogInformation("Nenhuma migração pendente encontrada");
            }

            await context.Database.MigrateAsync(cancellationToken);

            _logger.LogInformation("Migração do banco de dados concluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a migração automática do banco de dados: {ErrorMessage}", ex.Message);

            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}