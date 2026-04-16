using Microsoft.Extensions.Options;
using Serilog;

public class StaffApiKeyMiddleware : IMiddleware
{
    private readonly IConfiguration config;
    public StaffApiKeyMiddleware(IConfiguration _config)
    {
        config = _config;
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

        if (HasSkipAttribute(context))
        {
            await next(context);
            return;
        }

        var apiKey = context.Request.Headers["X-Staff-Api-Key"].ToString();
        var validKey = config["Api_Key"] ?? throw new ArgumentNullException("Api_Key is not configured");

        if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
        {
            Log.Warning("Unauthorized staff access attempt api_key:{ApiKeyId}", apiKey);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { error = "Staff access denied" });
            return;
        }

        await next(context);
    }

    private static bool HasSkipAttribute(HttpContext context) =>
        context.GetEndpoint()?.Metadata.GetMetadata<SkipStaffAuthAttribute>() is not null;
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class SkipStaffAuthAttribute : Attribute { }