using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Forwarder;
using System.Net;
using DotNetEnv;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using System.Threading.RateLimiting;

using MyApi.Models;
using Microsoft.Extensions.Options;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// --- Serilogの設定修正 ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new ExpressionTemplate(
        "[{@t:yyyy-MM-ddTHH:mm:ss.fffZ}] {@l:u4} " +
        "{#if RequestMethod is not null}" + // リクエストログ（RequestMethodプロパティがある場合）
            "access { {status: StatusCode, method: RequestMethod, path: RequestPath, ip: RemoteIpAddress, latency: Elapsed} }" +
        "{#else}" + // それ以外のログ（起動メッセージやエラーなど）
            "{@m}" +
        "{#end}\n"
    ))
    .CreateLogger();

// 環境変数のバインドと検証を追加
builder.Services.AddOptions<DotEnvSettings>()
    .BindConfiguration("DotEnv")  // appsettings.jsonの"DotEnv"セクション
    .ValidateDataAnnotations()
    .ValidateOnStart(); // 起動時に検証、設定漏れを即座に検出

// .env や設定ファイルから値を取得
int authLimit = builder.Configuration.GetValue("RateLimit_AuthLimit", 5);
int gameLimit = builder.Configuration.GetValue("RateLimit_GameLimit", 60);
int staffLimit = builder.Configuration.GetValue("RateLimit_StaffLimit", 10);
int cacheTtlMinutes = builder.Configuration.GetValue<int>("Cache_TtlMinutes", 5); // 例: 5

builder.Services.AddRateLimiter(options =>
{
    // ① IP単位のFixedWindow（メイン対策）
    // 認証系：厳しく（ブルートフォース対策）
    options.AddPolicy("AuthLimit", ctx =>
        RateLimitPartition.GetFixedWindowLimiter(GetClientIp(ctx), _ =>
            new FixedWindowRateLimiterOptions { PermitLimit = authLimit, Window = TimeSpan.FromMinutes(1) }));

    // ゲームアクション系：標準
    options.AddPolicy("GameLimit", ctx =>
    RateLimitPartition.GetTokenBucketLimiter(GetClientIp(ctx), _ =>
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
        RateLimitPartition.GetFixedWindowLimiter(GetClientIp(ctx), _ =>
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

// DB接続設定を追加 (SQLite)
//本番はpostgresqlに切り替える予定
builder.Services
    .AddDbContext<ApiDbContext>(options =>
        options.UseSqlite("Data Source=data.db"))
    .AddDbContext<StaffDbContext>(options =>
        options.UseSqlite("Data Source=data.db"));

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // .NET 9 標準の OpenAPI 登録

builder.Services.AddAuthentication(); // 認証サービスの追加（必要に応じて設定）
builder.Services.AddAuthorization();  // 認可サービスの追加（必要に応じて設定）
// PlayFab認証サービスの追加
builder.Services.AddHttpClient<PlayFabAuthService>();
builder.Services.AddSingleton<PlayFabAuthService>();
builder.Services.AddTransient<PlayFabAuthMiddleware>();
builder.Services.AddTransient<StaffApiKeyMiddleware>();
builder.Services.AddTransient<ServerStatusMiddleware>();

builder.Services.AddHostedService<TokenCleanupBackgroundService>(); // トークンクリーンアップのバックグラウンドサービスを追加

var app = builder.Build();

//例外処理のミドルウェアを追加（全体をキャッチしてログに残す）
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 開発用：詳細なエラー表示
}
else
{
    app.UseExceptionHandler("/error"); // 本番用：カスタムエラーページや共通処理へ
    app.UseHsts();  // 本番用：HSTSを有効化
}

//単に落とさないだけでなく、「なぜ落ちたか」を構造化ログ（Serilog）で残す
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // ここでSerilogを使って、スタックトレースをJSONで出力
        Log.Error(ex, "Unhandled exception during request: {Path}", context.Request.Path);
        throw; // ExceptionHandlerミドルウェアに流す
    }
});

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

app.UseRouting(); // ルーティングミドルウェアを追加

app.UseRateLimiter(); // レートリミットミドルウェアを追加

app.UseAuthentication();    // 認証ミドルウェア（必要に応じて追加）

app.UseAuthorization();     // 認可ミドルウェア（必要に応じて追加）

app.UseMiddleware<ServerStatusMiddleware>(); // サーバーステータス管理ミドルウェアを追加

app.UseMiddleware<StaffApiKeyMiddleware>(); // スタッフAPIキー認証ミドルウェアを追加

app.UseMiddleware<PlayFabAuthMiddleware>(); // PlayFab認証ミドルウェアを追加

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
    .DisableRateLimiting()
    .WithMetadata(new SkipServerStatusAttribute())
    .WithMetadata(new SkipPlayFabAuthAttribute())
    .WithMetadata(new SkipStaffAuthAttribute());

// 全ての /playfab/{*any} へのリクエストを PlayFab に転送
var transformer = new PlayFabForwardingTransformer();

app.Map("/playfab/{**catch-all}", async (HttpContext context, IHttpForwarder forwarder, IOptions<DotEnvSettings> settings) =>
{
    var error = await forwarder.SendAsync(context, settings.Value.BaseUrl, httpClient, requestConfig, transformer);

    if (error != ForwarderError.None)
    {
        var errorFeature = context.GetForwarderErrorFeature();
        Log.Warning("PlayFab proxy error: {Error}, Exception: {Exception}",
            error, errorFeature?.Exception?.Message);
    }
})
    .RequireRateLimiting("GameLimit")
    .WithMetadata(new SkipPlayFabAuthAttribute())
    .WithMetadata(new SkipStaffAuthAttribute());

app.MapControllers(); // 例: ゲームアクション系のレートリミットを適用
app.Run();

//-------------------------------------------------------------------
// IP取得ヘルパー（Cloud Run対応：X-Forwarded-Forを優先）
static string GetClientIp(HttpContext ctx)
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
        var clientIp = GetClientIp(httpContext);

        // X-Forwarded-Forを確実に上書き（ASP.NETが付加したものを排除）
        proxyRequest.Headers.Remove("X-Forwarded-For");
        proxyRequest.Headers.TryAddWithoutValidation("X-Forwarded-For", clientIp);

        // PlayFabが参照する可能性のある別ヘッダーにも設定
        proxyRequest.Headers.Remove("X-Real-IP");
        proxyRequest.Headers.TryAddWithoutValidation("X-Real-IP", clientIp);
    }

    private static string GetClientIp(HttpContext ctx)
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
// グローバルクラス（環境変数の読み込みと保持）
public sealed class DotEnvSettings
{
    public string TitleId { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string BaseUrl => $"https://{TitleId}.playfabapi.com";
    public string ApiKey { get; init; } = string.Empty;
}
public enum ServerStatus
{
    Running,
    Stopped,
    Maintenance
}
static class ServerStatusManager
{
    static volatile ServerStatus serverStatus = ServerStatus.Running; // 例: サーバーステータスを保持する変数
    static public ServerStatus CurrentServerStatus()
    {
        return serverStatus;
    }
    static public void ChangeServerStatus(ServerStatus status)
    {
        serverStatus = status;
    }
}