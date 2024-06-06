using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthClient.Pages
{
    public class RefreshTokenModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RefreshTokenModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return RedirectToPage("/Login");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("http://localhost:5000/api/auth/refresh-token",
                new StringContent(System.Text.Json.JsonSerializer.Serialize(new { refreshToken }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<LoginModel.AuthResponse>(responseData);

                HttpContext.Session.SetString("Token", result.Token);
                HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Refresh token failed.");
            return Page();
        }
    }
}
