using System.Data;
using System.Data.SqlClient;

namespace MoviesCastApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        
        public bool Register()
        {
            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_RegisterUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", Password);

            conn.Open();

            try
            {
                using var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    Id       = rdr.GetInt32(0);
                    UserName = rdr.GetString(1);
                    Email    = rdr.GetString(2);
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        
        public static User Login(string email, string password)
        {
            using var conn = Dal.GetConnection();
            using var cmd = new SqlCommand("sp_LoginUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            conn.Open();
            using var rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                return new User
                {
                    Id       = rdr.GetInt32(0),
                    UserName = rdr.GetString(1),
                    Email    = rdr.GetString(2)
                };
            }

            return null; 
        }
    }
}
