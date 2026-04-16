public class PlayFabAuthMiddleware(PlayFabAuthService authService) : IMiddleware
{
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

        var authHeader = context.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        if (!await authService.ValidateClientTokenAsync(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid token" });
            return;
        }

        await next(context);
    }

    private static bool HasSkipAttribute(HttpContext context) =>
        context.GetEndpoint()?.Metadata.GetMetadata<SkipPlayFabAuthAttribute>() is not null;
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class SkipPlayFabAuthAttribute : Attribute { }