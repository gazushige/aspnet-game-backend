using System.Security.Claims;
using Shared;
using MyApi.Handlers;


namespace MyApi.RpcServices
{
    public interface IRpcService<TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request, ClaimsPrincipal user);
    }
    public class GachaService : IRpcService<GachaRequest, GachaResponse>
    {
        public async Task<GachaResponse> ExecuteAsync(GachaRequest request, ClaimsPrincipal user)
        {
            // ここで実際のガチャロジックを実装する

            // ダミーのレスポンスを返す
            return new GachaResponse
            {
                Success = true,
            };
        }

    }


}