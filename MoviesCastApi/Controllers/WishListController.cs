using Microsoft.AspNetCore.Mvc;
using MoviesCastApi.Models;

namespace MoviesCastApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Movie>> Get() => Ok(Movie.ReadWishList());

        public class AddWishDto { public int Id { get; set; } }

        [HttpPost]
        public ActionResult Add([FromBody] AddWishDto dto)
        {
            if (dto == null) return BadRequest("Payload is required.");
            var added = Movie.AddToWishList(dto.Id);
            if (!added) return Conflict("Movie not found or already in wish list.");
            return Created($"/api/wishlist/{dto.Id}", new { dto.Id });
        }
    }
}
