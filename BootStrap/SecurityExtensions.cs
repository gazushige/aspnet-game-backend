using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace MyApi.Bootstrap
{
    public static class SecurityExtensions
    {
        public static WebApplicationBuilder AddSecurity(
            this WebApplicationBuilder builder)
        {
            var authLimit = builder.Configuration.GetValue("RateLimit_AuthLimit", 5);
            var gameLimit = builder.Configuration.GetValue("RateLimit_GameLimit", 60);
            var staffLimit = builder.Configuration.GetValue("RateLimit_StaffLimit", 10);

            // レートリミット
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("AuthLimit", ctx =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        HelperClass.GetClientIp(ctx), _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = authLimit,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("GameLimit", ctx =>
                    RateLimitPartition.GetTokenBucketLimiter(
                        HelperClass.GetClientIp(ctx), _ =>
                        new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = gameLimit,
                            TokensPerPeriod = gameLimit / 2,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                            AutoReplenishment = true,
                            QueueLimit = 0
                        }));

                options.AddPolicy("StaffLimit", ctx =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        HelperClass.GetClientIp(ctx), _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = staffLimit,
                            Window = TimeSpan.FromSeconds(1)
                        }));

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.Headers["Retry-After"] = "1";
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        """{"error":"rate_limit_exceeded","retryAfterSeconds":1}""",
                        cancellationToken);
                };
            });

            // 認証・認可
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/admin/login";
                    options.Cookie.Name = "Admin_Auth";
                });
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            // ミドルウェア
            builder.Services.AddTransient<PlayFabAuthMiddleware>();
            builder.Services.AddTransient<StaffApiKeyMiddleware>();
            builder.Services.AddTransient<ServerStatusMiddleware>();

            // PlayFab認証
            builder.Services.AddHttpClient<PlayFabAuthService>();
            builder.Services.AddSingleton<PlayFabAuthService>();
            builder.Services.AddHostedService<TokenCleanupBackgroundService>();

            return builder;
        }
    }
}