using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Movie>> GetAll()
        {
            var list = Movie.Read();
            return Ok(list);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Movie movie)
        {
            if (movie == null) return BadRequest("Movie payload is required.");
            var inserted = movie.Insert();
            if (inserted) return Created($"/api/movies/{movie.Id}", movie);
            return Conflict($"Movie with Id {movie.Id} already exists.");
        }

        [HttpGet("rating/{minRating:double}")]
        public ActionResult<List<Movie>> GetByRating(double minRating)
        {
            var filtered = Movie.ReadByRating(minRating);
            return Ok(filtered);
        }

        [HttpGet("duration")]
        public ActionResult<List<Movie>> GetByDuration([FromQuery] int maxDuration)
        {
            if (maxDuration < 1) return BadRequest("maxDuration must be >= 1.");
            var filtered = Movie.ReadByDuration(maxDuration);
            return Ok(filtered);
        }
    }
}
