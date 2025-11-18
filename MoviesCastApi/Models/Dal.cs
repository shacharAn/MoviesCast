using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MoviesCastApi.Models
{
    public static class Dal
    {
        private static string _connString;

        public static void Init(IConfiguration config)
        {
            _connString = config.GetConnectionString("MyConn");
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connString);
        }
    }
}
