using MySql.Data.MySqlClient;
using bvnote_api.Models;

namespace bvnote_api.Data
{
    public class BookContext
    {
        private readonly MySqlConnection _conn;
        public BookContext (DbContext dbContext)
        {
            _conn = dbContext.Connection();
        }
        public Book GetById(string id)
        {
            Book book = new();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM book WHERE BookID = @id", _conn);
            cmd.Parameters.AddWithValue("id", id);
            try
            {
                _conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    book.Id = rd.GetString(0);
                    book.Title = rd.GetString(1);
                    book.OldTestament = Convert.ToBoolean(rd.GetInt16(2));
                }
                rd.Close();
            } 
            catch (MySqlException e) { Console.Write(e); }
            finally { _conn.Close(); }
            return book;
        }

        public async Task<Book> GetByIdAsync(string id)
        {
            Book book = new();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM book WHERE BookID = @id", _conn);
            cmd.Parameters.AddWithValue("id", id);
            await _conn.OpenAsync();
            try
            {
                var rd = await cmd.ExecuteReaderAsync();
                if (await rd.ReadAsync())
                {
                    book.Id = rd.GetString(0);
                    book.Title = rd.GetString(1);
                    book.OldTestament = Convert.ToBoolean(rd.GetInt16(2));
                }
                await rd.CloseAsync();
            }
            catch (MySqlException e) { Console.Write(e); }
            await _conn.CloseAsync();
            return book;
        }

        public List<Book> GetList()
        {
            List<Book> books = new();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM book", _conn);
            try
            {
                _conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    books.Add(new Book()
                    {
                        Id = rd.GetString(0),
                        Title = rd.GetString(1),
                        OldTestament = rd.GetBoolean(2)
                    });
                }
                rd.Close();
            }
            catch (MySqlException e) { Console.Write(e); }
            finally { _conn.Close(); }
            return books;
        }

        public async Task<List<Book>> GetListAsync()
        {
            List<Book> books = new();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM book", _conn);
            try
            {
                await _conn.OpenAsync();
                var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    books.Add(new Book()
                    {
                        Id = rd.GetString(0),
                        Title = rd.GetString(1),
                        OldTestament = rd.GetBoolean(2)
                    });
                }
                await rd.CloseAsync();
            }
            catch (MySqlException e) { Console.Write(e); }
            await _conn.CloseAsync();
            return books;
        }

        public void Add(Book book)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO book (BookID, Title, OldTestament) VALUES (@id, @title, @ot)", _conn);
            cmd.Parameters.AddWithValue("id", book.Id);
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("ot", book.OldTestament);
            _conn.Open();
            try { cmd.ExecuteNonQuery(); }
            catch (MySqlException e) { Console.WriteLine(e); }
            finally { _conn.Close(); }
        }
    }
}
