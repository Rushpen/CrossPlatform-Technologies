using Microsoft.AspNetCore.Http;
using Gadelshin_Lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly Authorization _authorization;

        public AuthorizationController()
        {
            _authorization = new Authorization();
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Invalid username or password.");

            bool isAdmin = model.Login == "admin" && model.Password == "admin123";
            bool isUser = model.Login != "admin" && !string.IsNullOrEmpty(model.Password);

            if (!isAdmin && !isUser)
                return Unauthorized("Invalid credentials.");

            var token = _authorization.GenerateToken(model.Login, isAdmin);
            return Ok(new { Token = token, Role = isAdmin ? "admin" : "user" });
        }
        public class LoginModel
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
