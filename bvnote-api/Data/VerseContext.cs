using MySql.Data.MySqlClient;
using bvnote_api.Models;
using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel;

namespace bvnote_api.Data
{
    public class VerseContext
    {
        private readonly MySqlConnection _conn;
        private readonly BookContext _bookContext;
        public VerseContext(DbContext dbContext)
        {
            _conn = dbContext.Connection();
            _bookContext = new BookContext(dbContext);
        }

        public Verse GetById(Guid id)
        {
            Verse verse = new();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Verse WHERE VerseID = @id", _conn);
            cmd.Parameters.AddWithValue("id", id);
            MySqlDataReader rd = cmd.ExecuteReader();
            try
            {
                _conn.Open();
                rd.Read();
                verse.Id = rd.GetGuid(0);
                verse.ChapterNo = int.Parse(rd.GetString(1));
                verse.VerseNo = int.Parse(rd.GetString(2));
                verse.Content = rd.GetString(3);
                verse.BookId = rd.GetString(4);
            }
            catch (MySqlException e) { Console.Write(e); }
            finally { rd.Close(); _conn.Close(); }
            return verse;
        }
        public async Task<List<Verse>> GetBookVersesAsync(Book book)
        {
            List<Verse> verses = new List<Verse>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Verse WHERE BookID = @id", _conn);
            cmd.Parameters.AddWithValue("id", book.Id);
            try
            {
                await _conn.OpenAsync();
                var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    verses.Add(new Verse()
                    {
                        Id = rd.GetGuid(0),
                        ChapterNo = int.Parse(rd.GetString(1)),
                        VerseNo = int.Parse(rd.GetString(2)),
                        Content = rd.GetString(3),
                        BookId = rd.GetString(4),
                    });
                }
                await rd.CloseAsync();
            }
            catch (MySqlException e) { Console.WriteLine(e); }
            await _conn.CloseAsync();
            return verses;
        }
        public List<Verse> GetChapterVerses(Book book, int chapterNo)
        {
            List<Verse> verses = new List<Verse>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Verse WHERE BookID = @bookId AND ChapterNo = @chapterNo ORDER BY VerseNo", _conn);
            cmd.Parameters.AddWithValue("bookId", book.Id);
            cmd.Parameters.AddWithValue("chapterNo", chapterNo);
            try
            {
                _conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    verses.Add(new Verse()
                    {
                        Id = rd.GetGuid(0),
                        ChapterNo = int.Parse(rd.GetString(1)),
                        VerseNo = int.Parse(rd.GetString(2)),
                        Content = rd.GetString(3),
                        BookId = rd.GetString(4),
                    });
                }
                rd.Close();
                _conn.Close();
            }
            catch (MySqlException e) { Console.WriteLine(e); }
            return verses;
        }

        public async Task<List<Verse>> GetChapterVersesAsync(Book book, int chapterNo)
        {
            List<Verse> verses = new List<Verse>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Verse WHERE BookID = @bookId AND ChapterNo = @chapterNo ORDER BY VerseNo", _conn);
            cmd.Parameters.AddWithValue("bookId", book.Id);
            cmd.Parameters.AddWithValue("chapterNo", chapterNo);
            try
            {
                await _conn.OpenAsync();
                var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    verses.Add(new Verse()
                    {
                        Id = rd.GetGuid(0),
                        ChapterNo = int.Parse(rd.GetString(1)),
                        VerseNo = int.Parse(rd.GetString(2)),
                        Content = rd.GetString(3),
                        BookId = rd.GetString(4),
                    });
                }
                await rd.CloseAsync();
            }
            catch (MySqlException e) { Console.WriteLine(e); }
            await _conn.CloseAsync();
            return verses;
        }

        public void Add(Verse verse)
        {
            MySqlCommand cmd = new MySqlCommand(string.Format(
                "INSERT INTO verse (VerseID, Chapter, VerseNo, Content, BookID) VALUES ('%s', '%s', %s, '%s', '%s')"
                , verse.Id, verse.ChapterNo, verse.VerseNo, verse.Content, verse.BookId)
                , _conn);
            _conn.Open();
            try { cmd.ExecuteNonQuery(); }
            catch (MySqlException e) { Console.Write(e.Message); }
            finally { _conn.Close(); }
        }

        public void AddVerses(List<Verse> verses)
        {
            string sql = "INSERT INTO Verse (VerseID, ChapterNo, VerseNo, Content, BookID) VALUES (@id, @chap, @verse, @content, @book)";
            try 
            { 
                _conn.Open();
                MySqlTransaction transaction = _conn.BeginTransaction();
                foreach (Verse verse in verses) 
                {
                    MySqlCommand cmd = new MySqlCommand(sql, _conn);
                    cmd.Parameters.AddWithValue("@id", verse.Id);
                    cmd.Parameters.AddWithValue("@chap", verse.ChapterNo);
                    cmd.Parameters.AddWithValue("@verse", verse.VerseNo);
                    cmd.Parameters.AddWithValue("@content", verse.Content);
                    cmd.Parameters.AddWithValue("@book", verse.BookId);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (MySqlException e) { Console.Write(e); }
            finally { _conn.Close(); }
        }

        public void Remove(string id)
        {
            MySqlCommand cmd = new MySqlCommand(string.Format("DELETE FROM verse WHERE VerseID = '%s'", id), _conn);
            _conn.Open();
            try { cmd.ExecuteNonQuery(); }
            catch (MySqlException e) { Console.Write(e.Message); }
            finally { _conn.Close(); }
        }

        public void Populate()
        {
            List<Verse> verses = new List<Verse>();
            List<Book> books = _bookContext.GetList();
            // read and split data
            StreamReader rd = new StreamReader(@"C:\\Users\\jared\\source\\repos\\bvnote-api\\bvn-console\\resources\\NKJV_comma.txt");
            string line = rd.ReadLine();

            // iterate
            while (line != null)
            {
                string[] rst = line.Split(",");
                verses.Add(new Verse()
                {
                    Id = Guid.NewGuid(),
                    ChapterNo = Convert.ToInt32(rst[1]),
                    VerseNo = Convert.ToInt32(rst[2]),
                    Content = rst[3],
                    BookId = books[Convert.ToInt32(rst[0]) - 1].Id,
                });
                line = rd.ReadLine();
            }
            Console.WriteLine(verses[0].VerseNo);
            AddVerses(verses);
        }
    }
}
