using System.Text.Json;
using Google.Cloud.Firestore;

public class MetricsCollectorService : BackgroundService
{
    private readonly MetricsAggregator _aggregator;
    private readonly FirestoreDb _firestore;
    private readonly ILogger<MetricsCollectorService> _logger;

    // ✅ コンストラクタでDI注入 → null警告が全て消える
    public MetricsCollectorService(
        MetricsAggregator aggregator,
        FirestoreDb firestore,
        ILogger<MetricsCollectorService> logger)
    {
        _aggregator = aggregator;
        _firestore = firestore;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var minuteTask = RunMinutely(ct);
        var hourlyTask = RunHourly(ct);
        await Task.WhenAll(minuteTask, hourlyTask);
    }

    private async Task RunMinutely(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (await timer.WaitForNextTickAsync(ct))
        {
            try { _aggregator.FlushMinute(); }
            catch (Exception ex) { _logger.LogWarning(ex, "1分集計失敗"); }
        }
    }

    private async Task RunHourly(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (await timer.WaitForNextTickAsync(ct))
        {
            try
            {
                var snapshot = _aggregator.AggregateHour();
                var docId = snapshot.PeriodStart.ToString("yyyy-MM-dd_HH");

                // ✅ ctを削除
                await _firestore
                    .Collection("metrics_hourly")
                    .Document(docId)
                    .SetAsync(SnapshotToDict(snapshot));

                _logger.LogInformation("Firestoreに書き込み完了: {DocId}", docId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firestore書き込み失敗");
            }
        }
    }

    private static Dictionary<string, object> SnapshotToDict(MetricsSnapshot s)
    {
        var json = JsonSerializer.Serialize(s);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
    }
}