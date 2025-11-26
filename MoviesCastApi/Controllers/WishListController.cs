using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.BL;
using MoviesCastApi.Models;
using System.Linq;


namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private static readonly List<Movie> _wishList = new();

        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            return Ok(_wishList);
        }

        public class AddWishDto
        {
            public int Id { get; set; }
        }

        [HttpPost]
        public ActionResult Add([FromBody] AddWishDto dto)
        {
            if (dto == null)
                return BadRequest("Payload is required.");

            var allMovies = MoviesBL.GetAllMovies();
            var movie = allMovies.FirstOrDefault(m => m.Id == dto.Id);

            if (movie == null)
                return NotFound("Movie not found.");

            if (_wishList.Any(m => m.Id == dto.Id))
                return Conflict("Movie already in wish list.");

            _wishList.Add(movie);

            return Created($"/api/wishlist/{dto.Id}", new { dto.Id });
        }
    }
}
