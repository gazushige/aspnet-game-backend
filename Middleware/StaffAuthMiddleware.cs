using Microsoft.Extensions.Options;

public class StaffApiKeyMiddleware(IOptions<DotEnvSettings> option) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (HasSkipAttribute(context))
        {
            await next(context);
            return;
        }

        var apiKey = context.Request.Headers["X-Staff-Api-Key"].ToString();
        var validKey = option.Value.ApiKey;

        if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
        {
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