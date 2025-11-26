using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MoviesCastApi.Dal
{
    public class DBservice
    {
        private static readonly string ConnectionString;

        static DBservice()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            ConnectionString = config.GetConnectionString("MyConn")
                               ?? throw new Exception("Connection string 'MyConn' not found in appsettings.json");
        }

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
