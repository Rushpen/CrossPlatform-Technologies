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
            bool isValidUser = model.Login == "admin" && model.Password == "admin123";
            bool isAdmin = model.Login == "admin";

            if (!isValidUser)
                return Unauthorized("Invalid credentials.");

            var token = _authorization.GenerateToken(model.Login, isAdmin);
            return Ok(new { Token = token });
        }
        public class LoginModel
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
