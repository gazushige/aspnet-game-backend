using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyApi.Models;

namespace MyApi.Controllers
{
    /// <summary>
    /// API全般のベースコントローラー
    /// 共通の設定や機能を提供する。具体的なエンティティごとのコントローラーはCrudControllerを継承して作成する。
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("GameLimit")]
    public partial class ApiController(ApiDbContext dbContext, IConfiguration config) : ControllerBase
    {
        // 共通の機能や設定をここに追加可能
        private readonly ApiDbContext _db = dbContext;
        private readonly IConfiguration _config = config;
    }
}