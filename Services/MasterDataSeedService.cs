// MasterDataSeedService.cs
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using MyApi.Models;
public class MasterDataSeedService(IServiceProvider serviceProvider, MasterDataCache cache) : IHostedService
{
    // SeedService - StartAsync内をこれだけにできる
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ServerStatusManager.ChangeServerStatus(ServerStatus.Starting);
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

        // IMasterData実装クラスを全部自動スキャン
        var masterTypes = typeof(IEntity).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).IsAssignableFrom(t));

        foreach (var type in masterTypes)
        {
            await (Task)typeof(MasterDataSeedService)
                .GetMethod(nameof(LoadAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(type)
                .Invoke(this, [context])!;
        }

        ServerStatusManager.ChangeServerStatus(ServerStatus.Running);
    }

    private async Task LoadAsync<T>(ApiDbContext context) where T : class, IEntity
    {
        var dict = await context.Set<T>()
                                .AsNoTracking()
                                .ToListAsync()
                                .ContinueWith(t => t.Result.ToFrozenDictionary(i => i.Id));
        cache.Register(dict);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}