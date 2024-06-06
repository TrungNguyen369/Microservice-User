using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BffApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:5001/api/user/authenticate", data);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Unauthorized(errorMessage); // Trả về thông báo lỗi chi tiết
                }

                var user = await response.Content.ReadAsStringAsync();
                var userModel = JsonConvert.DeserializeObject<UserModel>(user);

                var token = GenerateJwtToken(userModel);
                var refreshToken = GenerateRefreshToken();

                userModel.RefreshToken = refreshToken;
                userModel.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                return Ok(new { Token = token, RefreshToken = refreshToken });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(UserModel user)
        {
            // Implement token generation logic here
            return "generated_token";
        }

        private string GenerateRefreshToken()
        {
            // Implement refresh token generation logic here
            return "generated_refresh_token";
        }
    }

    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
