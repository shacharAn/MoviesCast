using System.Linq;
namespace MoviesCastApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }       
        public double Income { get; set; }       
        public int ReleaseYear { get; set; }     
        public int Duration { get; set; }        
        public string Language { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string PhotoUrl { get; set; }
        public static List<Movie> MoviesList = new List<Movie>();

        public bool Insert()
        {
            if (MoviesList.Any(m => m.Id == this.Id))
                return false;

            MoviesList.Add(this);
            return true;
        }
        public static List<Movie> Read()
        {
            return MoviesList;
        }
        public static List<Movie> ReadByRating(double minRating)
        {
            List<Movie> filtered = new List<Movie>();
            foreach (var movie in MoviesList)
            {
                if (movie.Rating >= minRating)
                    filtered.Add(movie);
            }
            return filtered;
        }
        public static List<Movie> ReadByDuration(int maxDuration)
        {
            List<Movie> filtered = new List<Movie>();
            foreach (var movie in MoviesList)
            {
                if (movie.Duration <= maxDuration)
                    filtered.Add(movie);
            }
            return filtered;
        }
        public static List<Movie> WishList { get; } = new List<Movie>();

        public static bool AddToWishList(int id)
        {
            var movie = MoviesList.FirstOrDefault(m => m.Id == id);
            if (movie == null) return false;                
            if (WishList.Any(m => m.Id == id)) return false; 
            WishList.Add(movie);
            return true;
        }

        public static List<Movie> ReadWishList() => WishList;

    }
}
