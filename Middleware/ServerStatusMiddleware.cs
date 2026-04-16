public class ServerStatusMiddleware : IMiddleware
{
    private readonly IServerStatusService _serverStatusService;
    // コンストラクタでシングルトンサービスを受け取る
    public ServerStatusMiddleware(IServerStatusService serverStatusService)
    {
        _serverStatusService = serverStatusService;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Blazorはスキップ
        if (context.Request.Path.StartsWithSegments("/admin") ||
            context.Request.Path.StartsWithSegments("/_blazor") ||
            context.Request.Path.StartsWithSegments("/_framework"))
        {
            await next(context);
            return;
        }

        var status = _serverStatusService.CurrentStatus;
        if (status == ServerStatus.Running || HasSkipAttribute(context))
        {
            await next(context);
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 503;

        var label = status switch
        {
            ServerStatus.Maintenance => "down_for_maintenance",
            ServerStatus.Stopped => "stopped",
            ServerStatus.Starting => "starting",
            _ => "temporarily_unavailable",
        };

        await context.Response.WriteAsJsonAsync(new { status = label, timestamp = DateTime.UtcNow });
    }

    private static bool HasSkipAttribute(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        return endpoint?.Metadata.GetMetadata<SkipServerStatusAttribute>() is not null;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipServerStatusAttribute : Attribute { }