using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Cast>> GetAll() => Ok(Cast.Read());

        [HttpGet("{id}")]
        public ActionResult<Cast> Get(int id)
        {
            var c = Cast.Casts.FirstOrDefault(x => x.Id == id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Cast cast)
        {
            if (cast == null) return BadRequest("Cast payload is required.");
            var ok = cast.Insert();
            if (!ok) return Conflict($"Cast with Id {cast.Id} already exists.");
            return Created($"/api/casts/{cast.Id}", cast);
        }
    }
}
