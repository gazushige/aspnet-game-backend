using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using MyApi.Handlers;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Yarp.ReverseProxy.Forwarder;

namespace MyApi.Bootstrap
{
    public static class GameServicesExtensions
    {
        public static WebApplicationBuilder AddGameServices(
            this WebApplicationBuilder builder)
        {
            // RpcHandler 自動登録
            var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              i.GetGenericTypeDefinition() == typeof(IRpcHandler<,>)));
            foreach (var type in handlerTypes)
                builder.Services.AddScoped(type);

            // RpcService 自動登録
            var rpcServiceTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract && typeof(IRpcService).IsAssignableFrom(t));
            foreach (var type in rpcServiceTypes)
                builder.Services.AddScoped(type);

            builder.Services.AddScoped<RpcDispatcher>();

            // SignalR
            builder.Services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            });
            builder.Services.AddHostedService<ConnectionWatchdogService>();

            // Controller
            builder.Services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    if (!builder.Environment.IsDevelopment())
                    {
                        var toRemove = manager.ApplicationParts
                            .OfType<AssemblyPart>().First().Types
                            .Where(t => t.Name.Contains("Admin") ||
                                        t.Name.Contains("Staff"))
                            .ToList();
                        foreach (var type in toRemove)
                            manager.FeatureProviders
                                .Add(new RemoveControllerFeatureProvider(type));
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddOpenApi();
            builder.Services.AddHttpForwarder();
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();

            return builder;
        }

        public static WebApplication UseGameEndpoints(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAntiforgery();
            app.UseAuthorization();

            // Razor
            app.MapRazorComponents<rest.Components.App>()
                .AddInteractiveServerRenderMode();

            // PlayFabプロキシ
            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                ConnectTimeout = TimeSpan.FromSeconds(10),
            });
            var requestConfig = new ForwarderRequestConfig
            {
                ActivityTimeout = TimeSpan.FromSeconds(30)
            };
            var transformer = new PlayFabForwardingTransformer();

            // ヘルスチェック・ステータス
            app.MapGet("/health", () => """{"status":"ok"}""")
                .WithName("HealthCheck").DisableRateLimiting();
            app.MapGet("/status", (IServerStatusService s) =>
                    $$$"""{"status":"{{{s.CurrentStatus}}}"}""")
                .DisableRateLimiting();

            // 管理者ログイン
            app.MapPost("/admin/login-handler", async (
                HttpContext httpContext,
                IConfiguration config,
                [Microsoft.AspNetCore.Mvc.FromForm] string username,
                [Microsoft.AspNetCore.Mvc.FromForm] string password,
                [Microsoft.AspNetCore.Mvc.FromForm] string __RequestVerificationToken) =>
            {
                if (username == config["ADMIN_ID"] &&
                    password == config["ADMIN_PASSWORD"])
                {
                    var claims = new List<System.Security.Claims.Claim>
                    {
                        new(System.Security.Claims.ClaimTypes.Name, username)
                    };
                    var identity = new System.Security.Claims.ClaimsIdentity(
                        claims,
                        Microsoft.AspNetCore.Authentication.Cookies
                            .CookieAuthenticationDefaults.AuthenticationScheme);
                    await httpContext.SignInAsync(
                        Microsoft.AspNetCore.Authentication.Cookies
                            .CookieAuthenticationDefaults.AuthenticationScheme,
                        new System.Security.Claims.ClaimsPrincipal(identity));
                    return Results.Redirect("/admin");
                }
                return Results.Redirect("/admin/login?error=1");
            });

            app.MapPost("/admin/logout-handler", async (HttpContext httpContext) =>
            {
                await httpContext.SignOutAsync(
                    Microsoft.AspNetCore.Authentication.Cookies
                        .CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Redirect("/admin/login");
            });

            // ミドルウェア分岐
            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/staff"), branch =>
            {
                branch.UseRateLimiter();
                branch.UseMiddleware<StaffApiKeyMiddleware>();
            });
            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/playfab"), branch =>
            {
                branch.UseRateLimiter();
                branch.UseMiddleware<ServerStatusMiddleware>();
            });
            app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/chat"), branch =>
            {
                branch.UseMiddleware<ServerStatusMiddleware>();
                branch.UseMiddleware<PlayFabAuthMiddleware>();
            });

            // PlayFabプロキシ
            app.Map("/playfab/{**catch-all}", async (
                HttpContext context,
                IHttpForwarder forwarder,
                IConfiguration config) =>
            {
                var error = await forwarder.SendAsync(
                    context,
                    $"https://{config["TitleId"]}.playfabapi.com",
                    httpClient, requestConfig, transformer);

                if (error != ForwarderError.None)
                {
                    var f = context.GetForwarderErrorFeature();
                    Serilog.Log.Warning(
                        "PlayFab proxy error: {Error}, Exception: {Exception}",
                        error, f?.Exception?.Message);
                }
            }).RequireRateLimiting("GameLimit");

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            return app;
        }
    }
}