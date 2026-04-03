using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy.Forwarder;
using System.Net;
using MyApi.Models;
using Hangfire;
using Hangfire.Storage.SQLite;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

var builder = WebApplication.CreateBuilder(args);

// --- Serilogの設定修正 ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Hangfire", LogEventLevel.Warning) // ★これを追加：HangfireのINFOログを抑制
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

builder.Services.AddHttpForwarder(); // Forwarderを登録

builder.Host.UseSerilog();

// DB接続設定を追加 (SQLite)
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite("Data Source=gacha.db"));

// --- ここから追加・修正 ---
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // .NET 9 標準の OpenAPI 登録
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swashbuckle のジェネレーター

// Hangfire の設定
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage("hangfire.db")); // ジョブ管理用のDBファイル名

// Hangfire サーバー（バックグラウンド処理の実行体）を起動
builder.Services.AddHangfireServer();

// -----------------------

var app = builder.Build();

// --- ここから追加・修正 ---
if (app.Environment.IsDevelopment())
{
    // .NET 9 標準のエンドポイント表示（/openapi/v1.json）
    app.MapOpenApi();

    // 従来の Swagger UI 画面を表示（/swagger）
    app.UseSwagger();
    app.UseSwaggerUI();

    // Hangfire ダッシュボードを表示（/hangfire）
    app.UseHangfireDashboard();
}
// -----------------------
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

// PlayFab 用の HttpClient (接続をプールして再利用)
var httpClient = new HttpMessageInvoker(new SocketsHttpHandler
{
    UseProxy = false,
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    ConnectTimeout = TimeSpan.FromSeconds(10),
});

// プロキシのオプション（ホストヘッダーの書き換えなど）
var transformer = HttpTransformer.Default;
var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(30) };

// 全ての /playfab/{*any} へのリクエストを PlayFab に転送
app.Map("/playfab/{**catch-all}", async (HttpContext context, IHttpForwarder forwarder) =>
{
    // 送信先の PlayFab エンドポイントを構築
    // 例: https://{TitleId}.playfabapi.com/{path}
    var targetUri = "https://YOUR_TITLE_ID.playfabapi.com/";

    var error = await forwarder.SendAsync(context, targetUri, httpClient, requestConfig, transformer);

    if (error != ForwarderError.None)
    {
        // エラーハンドリング（ログ出力など）
    }
});

app.MapControllers();
app.Run();