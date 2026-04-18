using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, DateTime> _lastMessageTime = new();

    public async Task SendMessage(string message)
    {
        var userId = Context.UserIdentifier!;
        var now = DateTime.UtcNow;

        if (_lastMessageTime.TryGetValue(userId, out var last))
        {
            if ((now - last).TotalSeconds < 3)  // 3秒インターバル
            {
                await Clients.Caller.SendAsync("Error", "送信が早すぎます");
                return;
            }
        }

        _lastMessageTime[userId] = now;
        await Clients.All.SendAsync("ReceiveMessage", userId, message);
    }
}