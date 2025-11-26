using MoviesCastApi.Dal;
using MoviesCastApi.Models;


namespace MoviesCastApi.BL
{
    public class MoviesBL
    {
        public static List<Movie> GetAllMovies()
        {
            return MovieDal.GetAllMovies();
        }

        public static List<Movie> GetMoviesByRating(double minRating)
        {
            return MovieDal.GetMoviesByRating(minRating);
        }

        public static List<Movie> GetMoviesByDuration(int maxDuration)
        {
            return MovieDal.GetMoviesByDuration(maxDuration);
        }

        public static Movie InsertMovie(Movie movie)
        {
            return MovieDal.InsertMovie(movie);
        }
    }
}
