using System.Data;
using System.Data.SqlClient;

namespace MoviesCastApi.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public int MovieId { get; set; }
        public string PhotoUrl { get; set; }

        
        public static List<Cast> Read()
        {
            var list = new List<Cast>();

            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_GetAllCast", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new Cast
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    Role = rdr.GetString(2),
                    Age = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    MovieId = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4),
                    PhotoUrl = rdr.IsDBNull(5) ? null : rdr.GetString(5)
                });
            }

            return list;
        }

        
        public bool Insert()
        {
            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_InsertCast", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Role", Role);
            cmd.Parameters.AddWithValue("@Age", Age == 0 ? (object)DBNull.Value : Age);
            cmd.Parameters.AddWithValue("@MovieId", MovieId == 0 ? (object)DBNull.Value : MovieId);
            cmd.Parameters.AddWithValue("@PhotoUrl", string.IsNullOrWhiteSpace(PhotoUrl) ? (object)DBNull.Value : PhotoUrl);

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                Id = rdr.GetInt32(0);
                return true;
            }

            return false;
        }
    }
}
