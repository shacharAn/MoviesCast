using System.Linq;
namespace MoviesCastApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public long Income { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string PhotoUrl { get; set; }
    }
}