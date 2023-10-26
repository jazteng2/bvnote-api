using MySql.Data.MySqlClient;

namespace bvnote_api.Data
{
    public class DbContext
    {
        private readonly MySqlConnection _conn;

        public DbContext(MySqlConnection conn)
        {
            _conn = conn;
        }

        public MySqlConnection Connection()
        {
            return _conn;
        }
    }
}
