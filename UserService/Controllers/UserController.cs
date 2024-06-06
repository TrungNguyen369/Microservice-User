using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] User user)
        {
            _userService.Create(user);
            return Ok();
        }

        [HttpGet("validate-refresh-token")]
        public IActionResult ValidateRefreshToken(string username, string refreshToken)
        {
            var isValid = _userService.ValidateRefreshToken(username, refreshToken);
            if (!isValid)
            {
                return Unauthorized();
            }

            var user = _userService.GetByUsername(username);
            return Ok(user);
        }
    }

    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
