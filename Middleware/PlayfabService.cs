using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

public class PlayFabAuthService
{
    private readonly HttpClient _httpClient;
    private IOptions<DotEnvSettings> settings;
    private readonly string _secretKey;

    private string? _serverEntityToken;
    private DateTime _tokenExpiration;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    // 前回の話にあった ConcurrentDictionary
    private readonly ConcurrentDictionary<string, DateTime> _validatedTokens = new();

    public PlayFabAuthService(IOptions<DotEnvSettings> _settings, HttpClient httpClient)
    {
        settings = _settings;
        _httpClient = httpClient;
        _secretKey = _settings.Value.SecretKey;
    }

    // サーバー自身のトークンを確認・更新
    private async Task EnsureServerTokenAsync()
    {
        if (!string.IsNullOrEmpty(_serverEntityToken) && DateTime.UtcNow.AddMinutes(1) < _tokenExpiration)
            return;

        await _semaphore.WaitAsync();
        try
        {
            // Double-check
            if (!string.IsNullOrEmpty(_serverEntityToken) && DateTime.UtcNow.AddMinutes(1) < _tokenExpiration)
                return;

            var url = $"{settings.Value.BaseUrl}/Authentication/GetEntityToken";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-SecretKey", _secretKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync(PlayFabJsonContext.Default.EntityTokenResponse);
            _serverEntityToken = result!.Data.EntityToken;
            _tokenExpiration = result.Data.TokenExpiration;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // クライアントトークンの検証
    public async Task<bool> ValidateClientTokenAsync(string clientToken)
    {
        // 1. キャッシュ確認
        if (_validatedTokens.TryGetValue(clientToken, out var expiry) && expiry > DateTime.UtcNow)
        {
            return true;
        }

        // 2. PlayFabで検証
        await EnsureServerTokenAsync();

        var url = $"{settings.Value.BaseUrl}/Authentication/ValidateEntityToken";
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("X-EntityToken", _serverEntityToken);
        request.Content = JsonContent.Create(new ValidateRequest(clientToken), PlayFabJsonContext.Default.ValidateRequest);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return false;

        // 3. キャッシュに保存（23時間）
        _validatedTokens[clientToken] = DateTime.UtcNow.AddHours(23);
        return true;
    }

    // キャッシュ掃除用（BackgroundServiceから呼ぶ）
    public void CleanupCache()
    {
        var now = DateTime.UtcNow;
        foreach (var key in _validatedTokens.Keys)
        {
            if (_validatedTokens[key] < now) _validatedTokens.TryRemove(key, out _);
        }
    }
}
