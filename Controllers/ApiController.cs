using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyApi.Models;
using MyApi.Handlers;
using MyApi.RpcServices;
using Shared;
using System.Text.Json;
using Serilog;

namespace MyApi.Controllers
{
    /// <summary>
    /// API全般のベースコントローラー
    /// 共通の設定や機能を提供する。具体的なエンティティごとのコントローラーはCrudControllerを継承して作成する。
    /// </summary>
    [ApiController]
    [Route("rpc")]
    [EnableRateLimiting("GameLimit")]
    public class RpcController(
        ApiDbContext dbContext,
        IConfiguration config,
        RpcDispatcher dispatcher) : ControllerBase
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [HttpPost]
        public async Task<IActionResult> Handle([FromBody] JsonRpcEnvelope req)
        {
            // バリデーション
            if (req.Jsonrpc != "2.0")
                return Ok(RpcError(-32600, "Invalid Request", req?.Id));

            if (string.IsNullOrEmpty(req.Method))
                return Ok(RpcError(-32600, "Method is required", req?.Id));

            try
            {
                var result = await dispatcher.DispatchAsync(
                    req.Method, req.Params, User, dbContext, config);

                return Ok(new JsonRpcResponseEnvelope
                {
                    Jsonrpc = "2.0",
                    Result = result,
                    Id = req.Id
                });
            }
            catch (RpcException ex)
            {
                return Ok(RpcError(ex.Code, ex.Message, req.Id));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception. Method: {Method}", req.Method);
                return Ok(RpcError(-32603, "Internal error", req.Id));
            }
        }

        private static JsonRpcErrorEnvelope RpcError(int code, string message, string? id) =>
            new()
            {
                Jsonrpc = "2.0",
                Error = new JsonRpcError { Code = code, Message = message },
                Id = id
            };
    }

}