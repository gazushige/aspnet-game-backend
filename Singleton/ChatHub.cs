using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Serilog;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, DateTime> _lastMessageTime = new();

    // ← internal static に変更してWatchdogから参照可能にする
    internal static readonly ConcurrentDictionary<string, UserConnectionInfo> Connections = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier!;

        Connections[Context.ConnectionId] = new UserConnectionInfo
        {
            UserId = userId,
            ConnectedAt = DateTime.UtcNow,
            LastPingAt = DateTime.UtcNow,
        };

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Connections.TryRemove(Context.ConnectionId, out _);

        var reason = exception == null ? "正常切断" : $"異常切断: {exception.Message}";
        Log.Information("切断 UserId:{UserId} Reason:{Reason}",
            Context.UserIdentifier, reason);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task Ping()
    {
        if (Connections.TryGetValue(Context.ConnectionId, out var info))
        {
            info.LastPingAt = DateTime.UtcNow;
        }
        await Clients.Caller.SendAsync("Pong");
    }

    public async Task SendMessage(string message)
    {
        var userId = Context.UserIdentifier!;
        var now = DateTime.UtcNow;

        if (_lastMessageTime.TryGetValue(userId, out var last))
        {
            if ((now - last).TotalSeconds < 1)
            {
                await Clients.Caller.SendAsync("Error", "送信が早すぎます");
                return;
            }
        }

        _lastMessageTime[userId] = now;
        await Clients.All.SendAsync("ReceiveMessage", userId, message);
    }
}

public class UserConnectionInfo
{
    public string UserId { get; init; } = default!;
    public DateTime ConnectedAt { get; init; }
    public DateTime LastPingAt { get; set; }
}