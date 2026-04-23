// MasterDataSeedService.cs
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using MyApi.Models;
public class MasterDataSeedService(IServiceProvider serviceProvider, MasterDataCache cache,
    IServerStatusService statusService) // DIで受け取る
    : IHostedService
{
    // SeedService - StartAsync内をこれだけにできる
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        statusService.ChangeStatus(ServerStatus.Starting);
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

        // IEntity実装クラスを全部自動スキャン
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

        statusService.ChangeStatus(ServerStatus.Running);
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