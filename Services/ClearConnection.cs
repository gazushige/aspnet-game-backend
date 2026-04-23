using Microsoft.AspNetCore.SignalR;
using Serilog;

public class ConnectionWatchdogService(IHubContext<ChatHub> hubContext)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), ct);

            var threshold = DateTime.UtcNow - TimeSpan.FromMinutes(2);
            var zombies = ChatHub.Connections
                .Where(kvp => kvp.Value.LastPingAt < threshold)
                .ToList();

            foreach (var (connectionId, info) in zombies)
            {
                Log.Warning("ゾンビ接続を切断 UserId:{UserId}", info.UserId);
                await hubContext.Clients.Client(connectionId)
                    .SendAsync("ForceDisconnect", "接続タイムアウト", ct);
            }
        }
    }
}