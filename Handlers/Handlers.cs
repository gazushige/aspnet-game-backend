using System.Security.Claims;
using System.Text.Json;
using MyApi.Models;


namespace MyApi.Handlers
{
    // 引数なしのメソッド用のマーカー型（Sharedに置いてもOK）
    public sealed class NoParams { }

    public interface IRpcHandler<TRequest, TResponse>
    {
        string Method { get; }
        Task<JsonElement> HandleAsync(
            JsonElement? request,   // ← nullableに変更
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config);
    }

    public abstract class BaseRpcHandler<TRequest, TResponse>
        : IRpcHandler<TRequest, TResponse>
    {
        public abstract string Method { get; }

        public abstract Task<TResponse> ExecuteAsync(
            TRequest request,
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config);

        public async Task<JsonElement> HandleAsync(
            JsonElement? request,
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config)
        {
            var req = JsonSerializer.Deserialize<TRequest>(request?.ToString() ?? string.Empty, _options)
       ?? throw new RpcException(-32602, "Invalid params");

            var response = await ExecuteAsync(req, user, db, config);

            var json = JsonSerializer.Serialize(response, _options);  // ← オプション渡す
            return JsonDocument.Parse(json).RootElement;
        }

        // null・空・Null値・NoParamsを一括処理
        private static TRequest DeserializeOrDefault(JsonElement? element)
        {
            // TRequest が NoParams なら即返す
            if (typeof(TRequest) == typeof(NoParams))
                return (TRequest)(object)new NoParams();

            // null または JSON null または 空Object・空Array
            if (element is null
                || element.Value.ValueKind == JsonValueKind.Null
                || element.Value.ValueKind == JsonValueKind.Undefined)
            {
                // TRequestがnull許容ならnull、そうでなければデフォルトコンストラクタ
                return TryCreateDefault()
                    ?? throw new RpcException(-32602, "Invalid params: params is required");
            }

            return JsonSerializer.Deserialize<TRequest>(element.Value)
                ?? throw new RpcException(-32602, "Invalid params: deserialization failed");
        }

        // デフォルトインスタンスを試みる（引数なしコンストラクタがあれば生成）
        private static TRequest? TryCreateDefault()
        {
            try
            {
                return Activator.CreateInstance<TRequest>();
            }
            catch
            {
                return default;
            }
        }
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public class JsonRpcEnvelope
    {
        public string Jsonrpc { get; set; } = "2.0";
        public string Method { get; set; } = string.Empty;
        public JsonElement Params { get; set; }
        public string? Id { get; set; }
    }
    public class JsonRpcResponseEnvelope
    {
        public string Jsonrpc { get; set; } = "2.0";
        public JsonElement Result { get; set; }

        public string? Id { get; set; }
    }
    public class RpcException : Exception
    {
        public int Code { get; }
        public RpcException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}