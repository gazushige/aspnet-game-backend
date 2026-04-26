using System.Security.Claims;
using System.Text.Json;
using MyApi.Models;

namespace MyApi.Handlers
{
    public interface IRpcService { }
    public sealed class NoParams { }

    public interface IRpcHandler<TRequest, TResponse>
    {
        string Method { get; }
        Task<JsonElement> HandleAsync(
            JsonElement? request,
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config);
    }

    public abstract class BaseRpcHandler<TRequest, TResponse>
        : IRpcHandler<TRequest, TResponse>
    {
        // 起動時に1回だけ評価されるのでActivatorのコストはゼロ
        private static readonly bool _isNoParams = typeof(TRequest) == typeof(NoParams);

        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

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
            // DeserializeOrDefaultを通す（空文字・null対応はここで完結）
            var req = DeserializeOrDefault(request);

            var response = await ExecuteAsync(req, user, db, config);

            // JsonElementへの変換はSerialize→Parseの一方通行で統一
            using var doc = JsonDocument.Parse(JsonSerializer.Serialize(response, _options));
            return doc.RootElement.Clone(); // usingスコープを抜けても安全なようにClone
        }

        private static TRequest DeserializeOrDefault(JsonElement? element)
        {
            // NoParamsは即返す（リフレクション不要）
            if (_isNoParams)
                return (TRequest)(object)new NoParams();

            // null・Undefined・JSONのnull・空文字列は全てデフォルト扱い
            if (element is null
                || element.Value.ValueKind == JsonValueKind.Null
                || element.Value.ValueKind == JsonValueKind.Undefined
                || (element.Value.ValueKind == JsonValueKind.String
                    && string.IsNullOrEmpty(element.Value.GetString())))
            {
                return TryCreateDefault()
                    ?? throw new RpcException(-32602, "Invalid params: params is required");
            }

            // 文字列として来た場合（二重エンコード）は中身を再パース
            if (element.Value.ValueKind == JsonValueKind.String)
            {
                var str = element.Value.GetString()!;
                return JsonSerializer.Deserialize<TRequest>(str, _options)
                    ?? throw new RpcException(-32602, "Invalid params: deserialization failed");
            }

            // 通常のオブジェクト・配列
            return JsonSerializer.Deserialize<TRequest>(element.Value, _options)
                ?? throw new RpcException(-32602, "Invalid params: deserialization failed");
        }

        private static TRequest? TryCreateDefault()
        {
            try { return Activator.CreateInstance<TRequest>(); }
            catch { return default; }
        }
    }

    public class JsonRpcEnvelope
    {
        public string Jsonrpc { get; set; } = "2.0";
        public string Method { get; set; } = string.Empty;
        public JsonElement? Params { get; set; }  // nullableに統一
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
    public class JsonRpcErrorEnvelope
    {
        public string Jsonrpc { get; set; } = "2.0";
        public JsonRpcError Error { get; set; } = new();
        public string? Id { get; set; }
    }

    public class JsonRpcError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}