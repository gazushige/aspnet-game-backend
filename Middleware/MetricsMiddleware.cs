
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using System.Diagnostics;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MetricsAggregator _aggregator;

    // ✅ コンストラクタでDI注入
    public MetricsMiddleware(RequestDelegate next, MetricsAggregator aggregator)
    {
        _next = next;
        _aggregator = aggregator;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var sw = Stopwatch.StartNew();
        await _next(ctx);
        sw.Stop();

        // ✅ ルートテンプレートの正しい取得方法
        // 例: "GET /api/users/{id}" のように取得できる
        var routePattern = ctx.GetEndpoint()
            ?.Metadata
            .GetMetadata<RouteNameMetadata>()
            ?.RouteName
            ?? ctx.Features.Get<IRouteValuesFeature>()
                ?.RouteValues["action"]?.ToString()
            ?? ctx.Request.Path.Value
            ?? "unknown";

        var key = $"{ctx.Request.Method} {routePattern}";

        _aggregator.Record(key, ctx.Response.StatusCode, sw.Elapsed.TotalMilliseconds);

        if (ctx.Response.StatusCode >= 500)
        {
            Log.Error("5xx {Method} {Route} {StatusCode} {DurationMs}ms",
                ctx.Request.Method, routePattern,
                ctx.Response.StatusCode,
                sw.Elapsed.TotalMilliseconds);
        }
    }
}