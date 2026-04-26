using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using OpenTelemetry.Metrics;
using Serilog;
using Serilog.Events;
using Serilog.Templates;

namespace MyApi.Bootstrap
{
    public static class InfrastructureExtensions
    {
        public static WebApplicationBuilder AddInfrastructure(
            this WebApplicationBuilder builder)
        {
            // Firestore
            var projectId = builder.Configuration.GetValue("RateLimit_AuthLimit", "no-project-id");
            var firestoreDb = FirestoreDb.Create(projectId);
            builder.Services.AddSingleton(firestoreDb);

            // Serilog
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

            if (!builder.Environment.IsDevelopment())
                loggerConfig.WriteTo.Sink(new FirestoreSink(firestoreDb));

            Log.Logger = loggerConfig.CreateLogger();
            builder.Host.UseSerilog();

            // DbContext
            builder.Services
                .AddDbContext<AdminDbContext>(o =>
                    o.UseSqlite(builder.Configuration.GetConnectionString("AdminContext")))
                .AddDbContext<ApiDbContext>(o =>
                    o.UseSqlite(builder.Configuration.GetConnectionString("ApiContext"))
                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
                .AddDbContext<StaffDbContext>((sp, o) =>
                    o.UseSqlite(builder.Configuration.GetConnectionString("StaffContext")));

            // OpenTelemetry
            builder.Services.AddOpenTelemetry()
                .WithMetrics(m => m.AddRuntimeInstrumentation());

            // メトリクス
            builder.Services.AddSingleton<MetricsAggregator>();
            builder.Services.AddHostedService<MetricsCollectorService>();

            // キャッシュ
            builder.Services.AddSingleton<IServerStatusService, ServerStatusService>();
            builder.Services.AddSingleton<MasterDataCache>();
            builder.Services.AddHostedService<MasterDataSeedService>();

            builder.Services.AddHttpContextAccessor();

            return builder;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var traceHeader = httpContext.Request.Headers["X-Cloud-Trace-Context"]
                        .FirstOrDefault();
                    if (traceHeader != null)
                        diagnosticContext.Set(
                            "logging.googleapis.com/trace",
                            traceHeader.Split('/')[0]);
                };
            });

            app.UseRouting();
            app.UseStaticFiles();

            return app;
        }
    }
}