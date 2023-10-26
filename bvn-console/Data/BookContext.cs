using System.Data;
using System.Data.SQLite;
using System.IO.Pipelines;
using bvn_console.Model;

namespace bvn_console.Data
{
    public class BookContext
    {
        private readonly SQLiteDB _db;
        private readonly SQLiteConnection _conn;
        public BookContext(SQLiteDB db)
        {
            _db = db;
            _conn = db.Connection;
        }

        public Book GetById(string id)
        {
            Book book = new Book();
            SQLiteCommand cmd = new SQLiteCommand(_conn);
            cmd.CommandText = "SELECT * FROM Book WHERE BookID = @id";
            cmd.Parameters.AddWithValue("id", id);
            try
            {
                _conn.Open();
                SQLiteDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    book.Id = rd.GetString(0);
                    book.Title = rd.GetString(1);
                    book.OldTestament = Convert.ToBoolean(rd.GetInt32(2));
                }
                rd.Close();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex);
            }  
            finally { _conn.Close(); }
            return book;
        }

        public List<Book> GetAll()
        {
            List<Book> result = new List<Book>();
            SQLiteCommand cmd = new SQLiteCommand(_conn);
            cmd.CommandText = string.Format("SELECT * FROM Book");
            try
            {
                _conn.Open();
                SQLiteDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    result.Add(new Book()
                    {
                        Id = rd.GetString(0),
                        Title = rd.GetString(1),
                    });
                }
                rd.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { _conn.Close(); }
            return result;
        }
    }
}
