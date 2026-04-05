using Serilog;

public class TokenCleanupBackgroundService : BackgroundService
{
    private readonly PlayFabAuthService _authService; // 注入された ConcurrentDictionary 管理クラス
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(1);

    public TokenCleanupBackgroundService(PlayFabAuthService cache)
    {
        _authService = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Token cleanup background service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // 指定した間隔（1分）待機
                await Task.Delay(_cleanupInterval, stoppingToken);

                // PlayFabAuthService 内の掃除メソッドを直接実行
                _authService.CleanupCache();
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during token cleanup.");
            }
        }
    }
}