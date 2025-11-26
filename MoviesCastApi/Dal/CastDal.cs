using System.Data.SqlClient;
using MoviesCastApi.Models;

namespace MoviesCastApi.Dal
{
    public class CastDal
    {
        public static List<Cast> GetAllCast()
        {
            List<Cast> castList = new List<Cast>();

            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_GetAllCast", con))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cast c = new Cast
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),

                        Role = reader.IsDBNull(reader.GetOrdinal("Role"))
                               ? null
                               : reader.GetString(reader.GetOrdinal("Role")),

                        DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                                      ? (DateTime?)null
                                      : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),

                        Country = reader.IsDBNull(reader.GetOrdinal("Country"))
                                  ? null
                                  : reader.GetString(reader.GetOrdinal("Country")),

                        PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                   ? null
                                   : reader.GetString(reader.GetOrdinal("PhotoUrl"))
                    };

                    castList.Add(c);
                }
            }

            return castList;
        }

        public static Cast InsertCast(Cast cast)
        {
            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_InsertCast", con))
            {
                cmd.Parameters.AddWithValue("@Name", cast.Name);
                cmd.Parameters.AddWithValue("@Role", (object?)cast.Role ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateOfBirth", (object?)cast.DateOfBirth ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object?)cast.Country ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhotoUrl", (object?)cast.PhotoUrl ?? DBNull.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Cast inserted = new Cast
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Role = reader.IsDBNull(reader.GetOrdinal("Role"))
                                   ? null
                                   : reader.GetString(reader.GetOrdinal("Role")),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth"))
                                          ? (DateTime?)null
                                          : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                            Country = reader.IsDBNull(reader.GetOrdinal("Country"))
                                      ? null
                                      : reader.GetString(reader.GetOrdinal("Country")),
                            PhotoUrl = reader.IsDBNull(reader.GetOrdinal("PhotoUrl"))
                                       ? null
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
