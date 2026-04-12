using Google.Cloud.Firestore;
using Serilog.Core;
using Serilog.Events;

public class FirestoreSink : ILogEventSink
{
    private readonly FirestoreDb _db;

    // ✅ コンストラクタで注入
    public FirestoreSink(FirestoreDb db)
    {
        _db = db;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < LogEventLevel.Error) return;

        var doc = new Dictionary<string, object>
        {
            ["timestamp"] = logEvent.Timestamp.UtcDateTime,
            ["level"] = logEvent.Level.ToString(),
            ["message"] = logEvent.RenderMessage(),
            ["properties"] = logEvent.Properties
                .ToDictionary(p => p.Key, p => p.Value.ToString())
        };

        _ = _db.Collection("logs").AddAsync(doc);
    }
}