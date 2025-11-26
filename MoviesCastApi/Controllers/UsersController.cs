using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.BL;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User payload is required.");

            try
            {
                var registered = UsersBL.Register(user);
                return Created($"/api/users/{registered.Id}", registered);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Login payload is required.");

            var user = UsersBL.Login(dto.Email, dto.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            return Ok(user);
        }
    }
}
