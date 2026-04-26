using System.Security.Claims;
using Shared;
using MyApi.Models;
using MyApi.RpcServices;

namespace MyApi.Handlers
{

    // 引数ありのハンドラー例
    public class GachaExecuteHandler : BaseRpcHandler<GachaRequest, GachaResponse>
    {
        public override string Method => RpcMethods.GachaExecute;
        private readonly GachaService _gacha;

        public GachaExecuteHandler(GachaService gacha)
        {
            _gacha = gacha;
        }

        public override async Task<GachaResponse> ExecuteAsync(
            GachaRequest req,
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config)
        {
            return await _gacha.ExecuteAsync(req, user);
        }
    }
}