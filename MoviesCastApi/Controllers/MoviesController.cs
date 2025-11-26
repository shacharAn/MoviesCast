using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;
using MoviesCastApi.BL;


namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Movie>> GetAll()
        {
            var list = MoviesBL.GetAllMovies();
            return Ok(list);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Movie movie)
        {
            if (movie == null) return BadRequest("Movie payload is required.");
            var inserted = MoviesBL.InsertMovie(movie);

            return Created($"/api/movies/{inserted.Id}", inserted);
        }

        [HttpGet("rating/{minRating:double}")]
        public ActionResult<List<Movie>> GetByRating(double minRating)
        {
            var filtered = MoviesBL.GetMoviesByRating(minRating);
            return Ok(filtered);
        }

        [HttpGet("duration")]
        public ActionResult<List<Movie>> GetByDuration([FromQuery] int maxDuration)
        {
            if (maxDuration < 1)
                return BadRequest("maxDuration must be >= 1.");
            var filtered = MoviesBL.GetMoviesByDuration(maxDuration);
            return Ok(filtered);
        }
    }
}
