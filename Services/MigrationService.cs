using MyApi.Models;
using Microsoft.EntityFrameworkCore;

// マイグレーション実行後に自動で権限付与するサービス
public class MigrationService(
    IServiceScopeFactory scopeFactory,
    ILogger<MigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // ✅ Scopedサービスはスコープを作って解決する
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AdminDbContext>();

        await context.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("マイグレーション完了");

        await context.Database.ExecuteSqlRawAsync("""
            GRANT SELECT ON ALL TABLES IN SCHEMA app_schema TO api_group;
            GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA app_schema TO staff_group;
            GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA app_schema TO staff_group;
            """, cancellationToken);

        logger.LogInformation("権限付与完了");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}