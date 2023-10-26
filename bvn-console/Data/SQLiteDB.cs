using System.Data.SQLite;
using bvn_console.Model;
using System.IO;
using ZstdSharp.Unsafe;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace bvn_console.Data
{
    public class SQLiteDB
    {
        private readonly SQLiteConnection _conn;
        public SQLiteDB()
        {
            string cs = @"C:\Users\jared\source\repos\bvnote-api\bvn-console\resources\sqlite_db\bible.db";
            _conn = new SQLiteConnection($"Data Source={cs}");
            if (!File.Exists(cs)) 
            {
                SQLiteConnection.CreateFile("bible.db");
                Console.WriteLine("Does not exist");
            }
        }

        public SQLiteConnection Connection { get { return _conn; } }
    }
}
