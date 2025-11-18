using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Cast>> GetAll()
        {
            return Ok(Cast.Read());
        }

        [HttpGet("{id}")]
        public ActionResult<Cast> Get(int id)
        {
            var list = Cast.Read();
            var c = list.FirstOrDefault(x => x.Id == id);

            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Cast cast)
        {
            if (cast == null)
                return BadRequest("Cast payload is required.");

            var ok = cast.Insert();
            if (!ok)
                return Conflict($"Could not insert cast record.");

            return Created($"/api/casts/{cast.Id}", cast);
        }
    }
}
