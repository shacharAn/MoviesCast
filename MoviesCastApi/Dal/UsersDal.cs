using System.Data.SqlClient;
using MoviesCastApi.Models;

namespace MoviesCastApi.Dal
{
    public class UsersDal
    {
        public static User Register(User user)
        {
            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_RegisterUser", con))
            {
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User registered = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };

                        return registered;
                    }
                }
            }

            return null!;
        }

        public static User? Login(string email, string password)
        {
            using (SqlConnection con = DBservice.Connect())
            using (SqlCommand cmd = DBservice.CreateCommand("sp_LoginUser", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User loggedIn = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };

                        return loggedIn;
                    }
                }
            }

            return null;
        }
    }
}
