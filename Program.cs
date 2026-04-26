using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Forwarder;
using System.Net;
using DotNetEnv;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using System.Threading.RateLimiting;
using MyApi.Models;
using MyApi.Handlers;
using rest.Components;
using Google.Cloud.Firestore;
using OpenTelemetry.Metrics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;
using System.Reflection;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

string project_id = builder.Configuration.GetValue("RateLimit_AuthLimit", "no-project-id");
var firestoreDb = FirestoreDb.Create(project_id);


// DIにも登録（MetricsCollectorServiceが使うため）
builder.Services.AddSingleton(firestoreDb);

// --- Serilogの設定修正 ---
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new ExpressionTemplate(
        "[{@t:yyyy-MM-ddTHH:mm:ss.fffZ}] {@l:u4} " +
        "{#if RequestMethod is not null}" +
            "access { {status: StatusCode, method: RequestMethod, path: RequestPath, ip: RemoteIpAddress, latency: Elapsed} }" +
        "{#else}" +
            "{@m}" +
        "{#end}\n"
    ));

// ✅ 本番環境のみFirestoreに書き込む
if (!builder.Environment.IsDevelopment())
{
    loggerConfig.WriteTo.Sink(new FirestoreSink(firestoreDb));
}

Log.Logger = loggerConfig.CreateLogger();

// .env や設定ファイルから値を取得
int authLimit = builder.Configuration.GetValue("RateLimit_AuthLimit", 5);
int gameLimit = builder.Configuration.GetValue("RateLimit_GameLimit", 60);
int staffLimit = builder.Configuration.GetValue("RateLimit_StaffLimit", 10);
int cacheTtlMinutes = builder.Configuration.GetValue("Cache_TtlMinutes", 5); // 例: 5

builder.Services.AddRateLimiter(options =>
{
    // ① IP単位のFixedWindow（メイン対策）
    // 認証系：厳しく（ブルートフォース対策）
    options.AddPolicy("AuthLimit", ctx =>
        RateLimitPartition.GetFixedWindowLimiter(HelperClass.GetClientIp(ctx), _ =>
            new FixedWindowRateLimiterOptions { PermitLimit = authLimit, Window = TimeSpan.FromMinutes(1) }));

    // ゲームアクション系：標準
    options.AddPolicy("GameLimit", ctx =>
    RateLimitPartition.GetTokenBucketLimiter(HelperClass.GetClientIp(ctx), _ =>
        new TokenBucketRateLimiterOptions
        {
            // 1. バースト許容数（バケツの大きさ）
            TokenLimit = gameLimit,

            // 2. 回復量（1秒あたりの平均許容数）
            TokensPerPeriod = gameLimit / 2,

            // 3. 回復間隔
            ReplenishmentPeriod = TimeSpan.FromSeconds(1),

            // 4. 自動補充を有効にする
            AutoReplenishment = true,

            // 5. キューは使わず即座に429を返す
            QueueLimit = 0
        }));

    // 参照・ランキング系：緩め
    options.AddPolicy("StaffLimit", ctx =>
        RateLimitPartition.GetFixedWindowLimiter(HelperClass.GetClientIp(ctx), _ =>
            new FixedWindowRateLimiterOptions { PermitLimit = staffLimit, Window = TimeSpan.FromSeconds(1) }));

    // ② 429レスポンスのカスタマイズ
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers["Retry-After"] = "1";
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsync(
            """{"error":"rate_limit_exceeded","retryAfterSeconds":1}""",
            cancellationToken
        );
    };
});

builder.Services.AddHttpForwarder(); // Forwarderを登録

builder.Host.UseSerilog();

// アセンブリ内のIRpcHandler実装を全て自動登録
var handlerTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract && t.GetInterfaces()
        .Any(i => i.IsGenericType &&
                  i.GetGenericTypeDefinition() == typeof(IRpcHandler<,>)));

foreach (var type in handlerTypes)
    builder.Services.AddScoped(type);

// アセンブリ内のIRpcService実装を全て自動登録
var rpcServiceTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract && typeof(IRpcService).IsAssignableFrom(t));

foreach (var type in rpcServiceTypes)
    builder.Services.AddScoped(type);

builder.Services.AddScoped<RpcDispatcher>();


// --- DbContextの登録 ---
builder.Services
    .AddDbContext<AdminDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("AdminContext")))
    .AddDbContext<ApiDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("ApiContext"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
    .AddDbContext<StaffDbContext>((sp, options) =>
        options.UseSqlite(builder.Configuration.GetConnectionString("StaffContext")));

//本番環境では/adminと/staffのコントローラーを登録しない（セキュリティ強化のため）  
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        if (!builder.Environment.IsDevelopment())
        {
            var controllersToRemove = manager.ApplicationParts
                .OfType<AssemblyPart>()
                .First()
                .Types
                .Where(t => t.Name.Contains("Admin") || t.Name.Contains("Staff"))
                .ToList();

            foreach (var type in controllersToRemove)
            {
                manager.FeatureProviders.Add(new RemoveControllerFeatureProvider(type));
            }
        }
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddOpenApi(); // .NET 9 標準の OpenAPI 登録

// --- SignalRの登録 ---
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);   // サーバーからpingを送る間隔
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30); // これ以上無応答なら切断
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
});
builder.Services.AddHostedService<ConnectionWatchdogService>(); //SgnalRのゾンビ接続を定期的に切断

// --- 認証・認可の登録 ---
builder.Services.AddAuthentication(); // 認証サービスの追加（必要に応じて設定）
builder.Services.AddAuthorization();  // 認可サービスの追加（必要に応じて設定）

// PlayFab認証サービスの追加
builder.Services.AddHttpClient<PlayFabAuthService>();
builder.Services.AddSingleton<PlayFabAuthService>();
builder.Services.AddTransient<PlayFabAuthMiddleware>();
builder.Services.AddTransient<StaffApiKeyMiddleware>();
builder.Services.AddTransient<ServerStatusMiddleware>();

builder.Services.AddHostedService<TokenCleanupBackgroundService>(); // トークンクリーンアップのバックグラウンドサービスを追加

// --- サービスの登録 ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.Cookie.Name = "Admin_Auth";
    });
builder.Services.AddCascadingAuthenticationState(); // 認証状態をコンポーネントに伝播
builder.Services.AddHttpContextAccessor();

// キャッシュと初期化サービスの登録
builder.Services.AddSingleton<IServerStatusService, ServerStatusService>();
builder.Services.AddSingleton<MasterDataCache>();
builder.Services.AddHostedService<MasterDataSeedService>();

builder.Services.AddSingleton<MetricsAggregator>();
builder.Services.AddHostedService<MetricsCollectorService>();
// OTelはRuntime計測だけに絞る（HttpはMiddlewareで取るため）
builder.Services.AddOpenTelemetry()
    .WithMetrics(m => m.AddRuntimeInstrumentation());

//----------------------------------ここからapp--------------------------------------
var app = builder.Build();

// 1. エラーハンドリング（最優先）
if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
else { app.UseExceptionHandler("/error"); app.UseHsts(); }

// Serilog のリクエストロギングをカスタマイズして、Cloud RunのトレースIDを含める
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        // Cloud Runが付与するトレースIDを取得してログに含める
        var traceHeader = httpContext.Request.Headers["X-Cloud-Trace-Context"].FirstOrDefault();
        if (traceHeader != null)
        {
            diagnosticContext.Set("logging.googleapis.com/trace", traceHeader.Split('/')[0]);
        }
    };
});
app.UseRouting();

app.UseStaticFiles(); // ★ここに移動！認証や制限の前に静的ファイルを返す

// 6. 認証・認可（IdentityやCookieの解決）
app.UseAuthentication();
app.UseAntiforgery(); // ★認証と認可の間に置くのが公式の推奨
app.UseAuthorization();

//-------------------------- ここからMap -------------------------------------
// 8. 終端（エンドポイント）
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// PlayFab 用の HttpClient (接続をプールして再利用)
var httpClient = new HttpMessageInvoker(new SocketsHttpHandler
{
    UseProxy = false,
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    ConnectTimeout = TimeSpan.FromSeconds(10),
});

// プロキシのオプション（ホストヘッダーの書き換えなど）
var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(30) };

//healthチェック用のエンドポイント
app.MapGet("/health", () => "{\"status\":\"ok\"}").WithName("HealthCheck")
    .DisableRateLimiting();

//サーバーステータスを返すエンドポイント。running,maintenanceなど
app.MapGet("/status", (IServerStatusService statusService) =>
        $"{{\"status\":\"{statusService.CurrentStatus}\"}}\n")
    .DisableRateLimiting();

app.MapPost("/admin/login-handler", async (
    HttpContext httpContext,
    IConfiguration config,
    [FromForm] string username,
    [FromForm] string password,
    [FromForm] string __RequestVerificationToken) =>
{
    var adminId = config["ADMIN_ID"];
    var adminPass = config["ADMIN_PASSWORD"];

    if (username == adminId && password == adminPass)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return Results.Redirect("/admin");
    }

    return Results.Redirect("/admin/login?error=1");
});
app.MapPost("/admin/logout-handler", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/admin/login");
});


// --- 分岐 ---
// app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/rpc"), api =>
// {
//     api.UseRateLimiter();
//     api.UseMiddleware<ServerStatusMiddleware>();
//     api.UseMiddleware<PlayFabAuthMiddleware>();
// });

app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/staff"), staff =>
{
    staff.UseRateLimiter();
    staff.UseMiddleware<StaffApiKeyMiddleware>();
});

app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/playfab"), branch =>
{
    branch.UseRateLimiter();
    branch.UseMiddleware<ServerStatusMiddleware>();
});
// ① ルーティング

app.UseWhen(                         // ② 分岐ミドルウェア
    ctx => ctx.Request.Path.StartsWithSegments("/chat"),
    branch =>
    {
        branch.UseMiddleware<ServerStatusMiddleware>();   // メンテ判定
        branch.UseMiddleware<PlayFabAuthMiddleware>();    // 認証
    }
);

// 全ての /playfab/{*any} へのリクエストを PlayFab に転送
var transformer = new PlayFabForwardingTransformer();

app.Map("/playfab/{**catch-all}", async (HttpContext context, IHttpForwarder forwarder, IConfiguration config) =>
{
    var error = await forwarder.SendAsync(context, $"https://{config["TitleId"]}.playfabapi.com", httpClient, requestConfig, transformer);

    if (error != ForwarderError.None)
    {
        var errorFeature = context.GetForwarderErrorFeature();
        Log.Warning("PlayFab proxy error: {Error}, Exception: {Exception}",
            error, errorFeature?.Exception?.Message);
    }
}).RequireRateLimiting("GameLimit");

app.MapControllers();

app.MapHub<ChatHub>("/chat");  // SignalRの端点 (例: /chat)
app.Run();

// -------------------------------------------------------------------
// IP取得ヘルパー（Cloud Run対応：X-Forwarded-Forを優先）
public static class HelperClass
{
    public static string GetClientIp(HttpContext ctx)
    {
        // Cloud RunはGoogleのLBを経由するのでX-Forwarded-Forが信頼できる
        var forwarded = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwarded))
        {
            // 複数IPが連なる場合は先頭（元クライアント）を使う
            var firstIp = forwarded.Split(',')[0].Trim();
            if (IPAddress.TryParse(firstIp, out _))
                return firstIp;
        }
        return ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
// PlayFab用カスタムTransformer
public sealed class PlayFabForwardingTransformer : HttpTransformer
{
    public override async ValueTask TransformRequestAsync(
        HttpContext httpContext,
        HttpRequestMessage proxyRequest,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
        // デフォルトの変換を先に実行
        await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);

        // クライアントの生IPを取得（GetClientIpと同じロジック）
        var clientIp = HelperClass.GetClientIp(httpContext);

        // X-Forwarded-Forを確実に上書き（ASP.NETが付加したものを排除）
        proxyRequest.Headers.Remove("X-Forwarded-For");
        proxyRequest.Headers.TryAddWithoutValidation("X-Forwarded-For", clientIp);

        // PlayFabが参照する可能性のある別ヘッダーにも設定
        proxyRequest.Headers.Remove("X-Real-IP");
        proxyRequest.Headers.TryAddWithoutValidation("X-Real-IP", clientIp);
    }
}

// admin以外でのマイグレーションを禁止するクラス
public class DisabledMigrator : IMigrator
{
    private const string Message = "このContextではマイグレーションは禁止です。AdminDbContextを使用してください。";

    public string GenerateScript(
        string? fromMigration = null,
        string? toMigration = null,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        => throw new InvalidOperationException(Message);

    public void Migrate(string? targetMigration = null)
        => throw new InvalidOperationException(Message);

    public Task MigrateAsync(
        string? targetMigration = null,
        CancellationToken cancellationToken = default)
        => throw new InvalidOperationException(Message);

    // ✅ .NET9で追加されたメソッド
    // 未適用のマイグレーションがあるか確認するメソッドだが
    // このContextではマイグレーション自体禁止なので常にfalseを返す
    public bool HasPendingModelChanges() => false;
}

// 特定のコントローラーを削除するFeatureProvider
public class RemoveControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly Type _typeToRemove;

    public RemoveControllerFeatureProvider(Type typeToRemove)
    {
        _typeToRemove = typeToRemove;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var controller = feature.Controllers.FirstOrDefault(t => t.AsType() == _typeToRemove);
        if (controller != null)
        {
            feature.Controllers.Remove(controller);
        }
    }
}