using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public class RegisterDto
        {
            public string UserName { get; set; }
            public string Email    { get; set; }
            public string Password { get; set; }
        }

        public class LoginDto
        {
            public string Email    { get; set; }
            public string Password { get; set; }
        }

        // POST: api/users/register
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("All fields are required.");
            }

            var user = new User
            {
                UserName = dto.UserName.Trim(),
                Email    = dto.Email.Trim(),
                Password = dto.Password
            };

            var ok = user.Register();
            if (!ok)
            {
                return Conflict("User with this email already exists.");
            }

            return Created($"/api/users/{user.Id}", new
            {
                user.Id,
                user.UserName,
                user.Email
            });
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = User.Login(dto.Email.Trim(), dto.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email
            });
        }
    }
}
