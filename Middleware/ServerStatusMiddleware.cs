public class ServerStatusMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var status = ServerStatusManager.CurrentServerStatus(); // 1回だけ取得

        if (status == ServerStatus.Running || HasSkipAttribute(context))
        {
            await next(context);
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 503;

        var label = status switch
        {
            ServerStatus.Maintenance => "maintenance",
            ServerStatus.Stopped => "stopped",
            _ => "unavailable",
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