using System.Data;
using System.Data.SqlClient;

namespace MoviesCastApi.Dal
{
    public class DBservice
    {
        private const string ConnectionString = "Server=shachar\\SQLEXPRESS;Database=MoviesDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public static SqlConnection Connect()
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            return con;
        }

        public static SqlCommand CreateCommand(string storedProcedureName, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(storedProcedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }
    }
}
