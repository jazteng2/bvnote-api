using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace bvnote_api.Data
{
    public class MyContext : DbContext
    {
        private readonly string connectionString;
        private readonly MySqlConnection connection;
        public MyContext(MySqlConnection conn)
        {
            connection = conn;
            connectionString = conn.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
