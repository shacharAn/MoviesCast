using MoviesCastApi.Models;
using System.Data.SqlClient;

namespace MoviesCastApi.Dal
{
    public class MovieDal
    {
        public static List<Movie> GetAllMovies()
        {
            List<Movie> movies = new List<Movie>();

            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_GetAllMovies", con))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Movie m = new Movie
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Rating = reader.IsDBNull(reader.GetOrdinal("Rating"))
                                 ? 0
                                 : reader.GetDouble(reader.GetOrdinal("Rating")),
                        Income = reader.IsDBNull(reader.GetOrdinal("Income"))
                                 ? 0
                                 : reader.GetInt64(reader.GetOrdinal("Income")),
                        ReleaseYear = reader.IsDBNull(reader.GetOrdinal("ReleaseYear"))
                                      ? 0
                                      : reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                        Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                                   ? 0
                                   : reader.GetInt32(reader.GetOrdinal("Duration")),
                        Language = reader.IsDBNull(reader.GetOrdinal("Language"))
                                   ? string.Empty
                                   : reader.GetString(reader.GetOrdinal("Language")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                      ? string.Empty
                                      : reader.GetString(reader.GetOrdinal("Description")),
                        Genre = reader.IsDBNull(reader.GetOrdinal("Genre"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("Genre")),
                        PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                   ? string.Empty
                                   : reader.GetString(reader.GetOrdinal("PhotoUrl"))
                    };

                    movies.Add(m);
                }
            }

            return movies;
        }

        public static List<Movie> GetMoviesByRating(double minRating)
        {
            List<Movie> movies = new List<Movie>();

            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_GetMoviesByRating", con))
            {
                cmd.Parameters.AddWithValue("@MinRating", minRating);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Movie m = new Movie
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Rating = reader.IsDBNull(reader.GetOrdinal("Rating"))
                                     ? 0
                                     : reader.GetDouble(reader.GetOrdinal("Rating")),
                            Income = reader.IsDBNull(reader.GetOrdinal("Income"))
                                     ? 0
                                     : reader.GetInt64(reader.GetOrdinal("Income")),
                            ReleaseYear = reader.IsDBNull(reader.GetOrdinal("ReleaseYear"))
                                          ? 0
                                          : reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                            Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                                       ? 0
                                       : reader.GetInt32(reader.GetOrdinal("Duration")),
                            Language = reader.IsDBNull(reader.GetOrdinal("Language"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("Language")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                          ? string.Empty
                                          : reader.GetString(reader.GetOrdinal("Description")),
                            Genre = reader.IsDBNull(reader.GetOrdinal("Genre"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Genre")),
                            PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("PhotoUrl"))
                        };

                        movies.Add(m);
                    }
                }
            }

            return movies;
        }

        public static List<Movie> GetMoviesByDuration(int maxDuration)
        {
            List<Movie> movies = new List<Movie>();

            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_GetMoviesByDuration", con))
            {
                cmd.Parameters.AddWithValue("@MaxDuration", maxDuration);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Movie m = new Movie
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Rating = reader.IsDBNull(reader.GetOrdinal("Rating"))
                                     ? 0
                                     : reader.GetDouble(reader.GetOrdinal("Rating")),
                            Income = reader.IsDBNull(reader.GetOrdinal("Income"))
                                     ? 0
                                     : reader.GetInt64(reader.GetOrdinal("Income")),
                            ReleaseYear = reader.IsDBNull(reader.GetOrdinal("ReleaseYear"))
                                          ? 0
                                          : reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                            Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                                       ? 0
                                       : reader.GetInt32(reader.GetOrdinal("Duration")),
                            Language = reader.IsDBNull(reader.GetOrdinal("Language"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("Language")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                          ? string.Empty
                                          : reader.GetString(reader.GetOrdinal("Description")),
                            Genre = reader.IsDBNull(reader.GetOrdinal("Genre"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Genre")),
                            PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("PhotoUrl"))
                        };

                        movies.Add(m);
                    }
                }
            }

            return movies;
        }

        public static Movie InsertMovie(Movie movie)
        {
            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_InsertMovie", con))
            {
                cmd.Parameters.AddWithValue("@Title", movie.Title);
                cmd.Parameters.AddWithValue("@Rating", movie.Rating);
                cmd.Parameters.AddWithValue("@Income", movie.Income);
                cmd.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
                cmd.Parameters.AddWithValue("@Duration", movie.Duration);
                cmd.Parameters.AddWithValue("@Language", movie.Language ?? string.Empty);
                cmd.Parameters.AddWithValue("@Description", movie.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@Genre", movie.Genre ?? string.Empty);
                cmd.Parameters.AddWithValue("@PhotoUrl", movie.PhotoUrl ?? string.Empty);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Movie inserted = new Movie
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Rating = reader.IsDBNull(reader.GetOrdinal("Rating"))
                                     ? 0
                                     : reader.GetDouble(reader.GetOrdinal("Rating")),
                            Income = reader.IsDBNull(reader.GetOrdinal("Income"))
                                     ? 0
                                     : reader.GetInt64(reader.GetOrdinal("Income")),
                            ReleaseYear = reader.IsDBNull(reader.GetOrdinal("ReleaseYear"))
                                          ? 0
                                          : reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                            Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                                       ? 0
                                       : reader.GetInt32(reader.GetOrdinal("Duration")),
                            Language = reader.IsDBNull(reader.GetOrdinal("Language"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("Language")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                          ? string.Empty
                                          : reader.GetString(reader.GetOrdinal("Description")),
                            Genre = reader.IsDBNull(reader.GetOrdinal("Genre"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Genre")),
                            PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                       ? string.Empty
                                       : reader.GetString(reader.GetOrdinal("PhotoUrl"))
                        };

                        return inserted;
                    }
                }
            }

            return null!;
        }
    }
}

