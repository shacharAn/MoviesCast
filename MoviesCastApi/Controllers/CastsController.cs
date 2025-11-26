using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;
using MoviesCastApi.BL;


namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Cast>> GetAll()
        {
            var list = CastBL.GetAllCast();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<Cast> Get(int id)
        {
            var list = CastBL.GetAllCast();
            var cast = list.FirstOrDefault(c => c.Id == id);
            if (cast == null)
                return NotFound();

            return Ok(cast);
        }
        [HttpPost]
        public ActionResult Create([FromBody] Cast cast)
        {
            if (cast == null)
                return BadRequest("Cast payload is required.");

            if (string.IsNullOrWhiteSpace(cast.Name))
                return BadRequest("Name is required.");

            var inserted = CastBL.InsertCast(cast);

            return Created($"/api/casts/{inserted.Id}", inserted);
        }
    }
}
