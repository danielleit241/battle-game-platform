namespace BattleGame.MigrationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _serviceProvider;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    private async Task EnsureDatabaseAsync<T>(T context, CancellationToken stoppingToken) where T : DbContext
    {
    }

    private async Task MigrateAsync<T>(T context, CancellationToken stoppingToken) where T : DbContext
    {
    }

    private async Task SeedAsync<T>(T context, CancellationToken stoppingToken) where T : DbContext
    {
    }
}
