using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JwtAuthClient.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("RefreshToken");

            return RedirectToPage("/Login");
        }
    }
}
