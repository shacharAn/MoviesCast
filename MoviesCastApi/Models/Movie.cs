using System.Data;
using System.Linq;
using System.Data.SqlClient;

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

        
        public static List<Movie> Read()
        {
            var list = new List<Movie>();

            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_GetAllMovies", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new Movie
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.GetString(1),
                    Rating = rdr.GetDouble(2),
                    Income = rdr.GetInt64(3),
                    ReleaseYear = rdr.GetInt32(4),
                    Duration = rdr.GetInt32(5),
                    Language = rdr.GetString(6),
                    Description = rdr.GetString(7),
                    Genre = rdr.GetString(8),
                    PhotoUrl = rdr.GetString(9)
                });
            }

            return list;
        }

        
        public bool Insert()
        {
            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_InsertMovie", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Rating", Rating);
            cmd.Parameters.AddWithValue("@Income", Income);
            cmd.Parameters.AddWithValue("@ReleaseYear", ReleaseYear);
            cmd.Parameters.AddWithValue("@Duration", Duration);
            cmd.Parameters.AddWithValue("@Language", Language);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Genre", Genre);
            cmd.Parameters.AddWithValue("@PhotoUrl", PhotoUrl);

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                Id = rdr.GetInt32(0);
                return true;
            }

            return false;
        }

        
        public static List<Movie> ReadByRating(double minRating)
        {
            var list = new List<Movie>();

            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_GetMoviesByRating", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MinRating", minRating);

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new Movie
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.GetString(1),
                    Rating = rdr.GetDouble(2),
                    Income = rdr.GetInt64(3),
                    ReleaseYear = rdr.GetInt32(4),
                    Duration = rdr.GetInt32(5),
                    Language = rdr.GetString(6),
                    Description = rdr.GetString(7),
                    Genre = rdr.GetString(8),
                    PhotoUrl = rdr.GetString(9)
                });
            }

            return list;
        }

        public static List<Movie> ReadByDuration(int maxDuration)
        {
            var list = new List<Movie>();

            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_GetMoviesByDuration", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MaxDuration", maxDuration);

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new Movie
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.GetString(1),
                    Rating = rdr.GetDouble(2),
                    Income = rdr.GetInt64(3),
                    ReleaseYear = rdr.GetInt32(4),
                    Duration = rdr.GetInt32(5),
                    Language = rdr.GetString(6),
                    Description = rdr.GetString(7),
                    Genre = rdr.GetString(8),
                    PhotoUrl = rdr.GetString(9)
                });
            }

            return list;
        }
        private static readonly List<Movie> WishList = new List<Movie>();

        public static bool AddToWishList(int id)
        {
            var all = Read();
            var movie = all.FirstOrDefault(m => m.Id == id);
            if (movie == null) return false;              // אין סרט כזה
            if (WishList.Any(m => m.Id == id)) return false; // כבר קיים ב-WishList

            WishList.Add(movie);
            return true;
        }

        public static List<Movie> ReadWishList()
        {
            return WishList;
        }
    }
}
