using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "{\"message\":\"Hello, World!\"}";
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class ReverseController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            // HttpClientは静的またはDIで管理するのが理想ですが、テスト用としてusingで囲みます
            using (var client = new HttpClient())
            {
                try
                {
                    // WSL2上のGoサーバー（healthエンドポイント）を叩く
                    // Localhost Forwardingが効いていれば localhost でOK
                    var response = await client.GetAsync("http://localhost:8080/health");

                    // ステータスコードが成功(200-299)でない場合は例外を投げる
                    response.EnsureSuccessStatusCode();

                    // レスポンスボディを文字列として読み取る
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    // エラーが発生した場合はその内容を返す
                    return $"環境エラー: {e.Message}。WSL2側でサーバーが起動しているか確認してください。";
                }
            }
        }
    }
}