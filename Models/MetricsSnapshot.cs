// Models/MetricsSnapshot.cs
public class MetricsSnapshot
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string PeriodLabel { get; set; } = ""; // "1m", "1h", "1d"

    public HttpMetrics Http { get; set; } = new();
    public RuntimeMetrics Runtime { get; set; } = new();
    public Dictionary<string, EndpointMetrics> Endpoints { get; set; } = new();
}

public class HttpMetrics
{
    public long TotalRequests { get; set; }
    public double RpsAvg { get; set; }
    public double RpsMax { get; set; }
    public double P50Ms { get; set; }
    public double P95Ms { get; set; }
    public double P99Ms { get; set; }
    public long Error4xx { get; set; }
    public long Error5xx { get; set; }
    public double ErrorRate => TotalRequests == 0 ? 0
        : (double)(Error4xx + Error5xx) / TotalRequests;
}

public class RuntimeMetrics
{
    public int GcGen2Count { get; set; }
    public double HeapMbAvg { get; set; }
    public int ThreadpoolQueueMax { get; set; }
    public long ExceptionCount { get; set; }
}

public class EndpointMetrics
{
    public long Count { get; set; }
    public double P95Ms { get; set; }
    public long Errors { get; set; }
}