using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthClient.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new { Username, Password };
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:5002/api/auth/login", data);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthResponse>(responseData);

                HttpContext.Session.SetString("Token", result.Token);
                HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Login failed.");
            return Page();
        }

        public class AuthResponse
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
