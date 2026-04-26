using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyApi.Models;
using MyApi.Handlers;
using MyApi.RpcServices;
using Shared;
using System.Text.Json;

namespace MyApi.Controllers
{
    /// <summary>
    /// API全般のベースコントローラー
    /// 共通の設定や機能を提供する。具体的なエンティティごとのコントローラーはCrudControllerを継承して作成する。
    /// </summary>

    [ApiController]
    [Route("rpc")]
    [EnableRateLimiting("GameLimit")]
    public partial class ApiController(ApiDbContext dbContext, IConfiguration config) : ControllerBase
    {
        // 共通の機能や設定をここに追加可能
        private readonly ApiDbContext _db = dbContext;
        private readonly IConfiguration _config = config;

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonRpcEnvelope req)
        {
            // ここでリクエストを処理する
            // 例えば、Methodに基づいて適切なハンドラーを呼び出すなど
            // ハンドラーはDIコンテナから取得することも可能
            if (req.Jsonrpc != "2.0")
            {
                return BadRequest(new { error = "Invalid Request" });
            }

            if (string.IsNullOrEmpty(req.Method))
            {
                return BadRequest(new { error = "Method is required" });
            }

            var response = await ProcessRequestAsync(req);
            return Ok(response);
        }

        private async Task<JsonRpcResponseEnvelope> ProcessRequestAsync(JsonRpcEnvelope req)
        {
            switch (req.Method)
            {
                case RpcMethods.GachaExecute:
                    var handler = new GachaExecuteHandler(new GachaService());
                    var response = await handler.HandleAsync(req.Params, User, _db, _config);
                    return new JsonRpcResponseEnvelope
                    {
                        Jsonrpc = "2.0",
                        Id = req.Id,
                        Result = response
                    };

                default:
                    return await Task.FromResult(new JsonRpcResponseEnvelope
                    {
                        Jsonrpc = "2.0",
                        Id = req.Id,
                        Result = JsonDocument.Parse(JsonSerializer.Serialize(new { error = "Method not found" })).RootElement
                    });
            }
        }
    }

}