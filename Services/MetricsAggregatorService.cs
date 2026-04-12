// Services/MetricsAggregator.cs
public class MetricsAggregator
{
    // 直近60分分の1分スナップショットをローリング保持
    private readonly Queue<MetricsSnapshot> _minuteBuffer = new();
    private readonly object _lock = new();

    // OTelのInMemoryExporterから受け取ったRawデータをここに蓄積
    private readonly List<(string route, int status, double durationMs)> _rawRequests = new();

    public void Record(string route, int statusCode, double durationMs)
    {
        lock (_lock)
        {
            _rawRequests.Add((route, statusCode, durationMs));
        }
    }

    // 1分ごとにBackgroundServiceから呼ぶ
    public MetricsSnapshot FlushMinute()
    {
        List<(string route, int status, double durationMs)> batch;
        lock (_lock)
        {
            // ✅ 修正後
            batch = [.. _rawRequests];
            _rawRequests.Clear();
        }

        var now = DateTime.UtcNow;
        var snapshot = BuildSnapshot(batch, now.AddMinutes(-1), now, "1m");

        lock (_lock)
        {
            _minuteBuffer.Enqueue(snapshot);
            // 直近60件（1時間分）だけ保持
            while (_minuteBuffer.Count > 60)
                _minuteBuffer.Dequeue();
        }

        return snapshot;
    }

    // 1時間分を集約してFirestore用スナップショットを生成
    public MetricsSnapshot AggregateHour()
    {
        lock (_lock)
        {
            var minutes = _minuteBuffer.ToList();
            if (minutes.Count == 0) return new MetricsSnapshot();

            // 全durationを復元して再集計
            var allDurations = new List<double>();
            long total4xx = 0, total5xx = 0, totalReq = 0;
            double rpsMax = 0;
            var endpointData = new Dictionary<string, List<double>>();

            foreach (var m in minutes)
            {
                totalReq += m.Http.TotalRequests;
                total4xx += m.Http.Error4xx;
                total5xx += m.Http.Error5xx;
                rpsMax = Math.Max(rpsMax, m.Http.RpsMax);
                // エンドポイント別集約
                foreach (var (ep, em) in m.Endpoints)
                {
                    if (!endpointData.ContainsKey(ep))
                        endpointData[ep] = new List<double>();
                    // P95をそのまま使う（近似で十分）
                }
            }

            return new MetricsSnapshot
            {
                PeriodStart = minutes.First().PeriodStart,
                PeriodEnd = minutes.Last().PeriodEnd,
                PeriodLabel = "1h",
                Http = new HttpMetrics
                {
                    TotalRequests = totalReq,
                    RpsAvg = totalReq / 3600.0,
                    RpsMax = rpsMax,
                    P50Ms = minutes.Average(m => m.Http.P50Ms),
                    P95Ms = minutes.Max(m => m.Http.P95Ms), // 最悪値を保守的に採用
                    P99Ms = minutes.Max(m => m.Http.P99Ms),
                    Error4xx = total4xx,
                    Error5xx = total5xx
                },
                // RuntimeはLastの値（瞬間値で十分）
                Runtime = minutes.Last().Runtime,
                Endpoints = AggregatEndpoints(minutes)
            };
        }
    }

    private MetricsSnapshot BuildSnapshot(
        List<(string route, int status, double durationMs)> batch,
        DateTime start, DateTime end, string label)
    {
        var durations = batch.Select(r => r.durationMs).OrderBy(d => d).ToList();
        var byRoute = batch.GroupBy(r => r.route);

        return new MetricsSnapshot
        {
            PeriodStart = start,
            PeriodEnd = end,
            PeriodLabel = label,
            Http = new HttpMetrics
            {
                TotalRequests = batch.Count,
                RpsAvg = batch.Count / 60.0,
                RpsMax = 0, // 1分粒度ではRPSの変動は別途計測が必要
                P50Ms = Percentile(durations, 0.50),
                P95Ms = Percentile(durations, 0.95),
                P99Ms = Percentile(durations, 0.99),
                Error4xx = batch.Count(r => r.status is >= 400 and < 500),
                Error5xx = batch.Count(r => r.status >= 500)
            },
            Endpoints = byRoute.ToDictionary(
                g => g.Key,
                g =>
                {
                    var d = g.Select(r => r.durationMs).OrderBy(x => x).ToList();
                    return new EndpointMetrics
                    {
                        Count = g.Count(),
                        P95Ms = Percentile(d, 0.95),
                        Errors = g.Count(r => r.status >= 400)
                    };
                })
        };
    }

    private static double Percentile(List<double> sorted, double p)
    {
        if (sorted.Count == 0) return 0;
        int idx = (int)Math.Ceiling(p * sorted.Count) - 1;
        return sorted[Math.Max(0, Math.Min(idx, sorted.Count - 1))];
    }

    private Dictionary<string, EndpointMetrics> AggregatEndpoints(
        List<MetricsSnapshot> minutes)
    {
        return minutes
            .SelectMany(m => m.Endpoints)
            .GroupBy(kv => kv.Key)
            .ToDictionary(
                g => g.Key,
                g => new EndpointMetrics
                {
                    Count = g.Sum(kv => kv.Value.Count),
                    P95Ms = g.Max(kv => kv.Value.P95Ms),
                    Errors = g.Sum(kv => kv.Value.Errors)
                });
    }
}