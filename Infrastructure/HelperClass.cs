using System.Net;

namespace MyApi.Bootstrap
{
    public static class HelperClass
    {
        public static string GetClientIp(HttpContext ctx)
        {
            var forwarded = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                var firstIp = forwarded.Split(',')[0].Trim();
                if (IPAddress.TryParse(firstIp, out _))
                    return firstIp;
            }
            return ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}