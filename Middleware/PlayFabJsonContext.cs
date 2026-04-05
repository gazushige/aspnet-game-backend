using System.Text.Json.Serialization;



// 1. シリアライズ対象のデータ構造（record または class）
public record EntityTokenResponse(EntityTokenData Data);
public record EntityTokenData(string EntityToken, DateTime TokenExpiration);
public record ValidateRequest(string EntityToken);

// 2. ソースジェネレーターの定義
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default // 反射を使わないモードを明示
)]
[JsonSerializable(typeof(EntityTokenResponse))]
[JsonSerializable(typeof(ValidateRequest))]
// エラー回避のため、基本型も必要に応じて追加
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DateTime))]
internal partial class PlayFabJsonContext : JsonSerializerContext
{ }
