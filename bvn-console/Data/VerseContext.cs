using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using bvn_console.Model;
using Org.BouncyCastle.Asn1.Mozilla;

namespace bvn_console.Data
{
    public class VerseContext
    {
        private readonly SQLiteDB _db;
        private readonly SQLiteConnection _conn;
        private readonly BookContext _bookContext;
        public VerseContext(SQLiteDB db, BookContext bookContext)
        {
            _db = db;
            _conn = db.Connection;
            _bookContext = bookContext;
        }

        public List<Verse> GetChapterVerses(string bookId, int chapNo)
        {
            List<Verse> result = new List<Verse>();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Verse WHERE BookID = @bookId AND ChapterNo = @chapNo", _conn);
            cmd.Parameters.AddWithValue("@bookId", bookId);
            cmd.Parameters.AddWithValue("@chapNo", chapNo);
            try
            {
                _conn.Open();
                SQLiteDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    result.Add(new Verse()
                    {
                        Id = rd.GetString(0),
                        ChapterNo = rd.GetInt32(1),
                        VerseNo = rd.GetInt32(2),
                        Content = rd.GetString(3),
                        BookID = rd.GetString(4),
                    });
                }
                rd.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            finally { _conn.Close(); }

            foreach (Verse v in result)
            {
                v.Book = _bookContext.GetById(v.BookID);
            }

            return result; 
        }

        public void AddVerse(Verse verse)
        {
            string qry = "INSERT INTO Verse (VerseID, ChapterNo, VerseNo, Content, BookID) VALUES (@id, @chap, @verse, @content, @book)";
            SQLiteCommand cmd = new SQLiteCommand(qry, _conn);
            cmd.Parameters.AddWithValue("@id", verse.Id);
            cmd.Parameters.AddWithValue("@chap", verse.ChapterNo);
            cmd.Parameters.AddWithValue("@verse", verse.VerseNo);
            cmd.Parameters.AddWithValue("@content", verse.Content);
            cmd.Parameters.AddWithValue("@book", verse.BookID);
            
            try
            {
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch (SQLiteException ex) { Console.WriteLine(ex); }

        }

        public void AddVerses(List<Verse> verses)
        {
            string qry = "INSERT INTO Verse (VerseID, ChapterNo, VerseNo, Content, BookID) VALUES (@id, @chap, @verse, @content, @book)";
            SQLiteCommand cmd = new SQLiteCommand(qry, _conn);

            try
            {
                _conn.Open();
                SQLiteTransaction transaction = _conn.BeginTransaction();
                foreach (Verse verse in verses)
                {
                    cmd.Parameters.AddWithValue("@id", verse.Id);
                    cmd.Parameters.AddWithValue("@chap", verse.ChapterNo);
                    cmd.Parameters.AddWithValue("@verse", verse.VerseNo);
                    cmd.Parameters.AddWithValue("@content", verse.Content);
                    cmd.Parameters.AddWithValue("@book", verse.BookID);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Completed Input - Now in book: " + verse.BookID);
                }
                transaction.Commit();
                _conn.Close();
            }
            catch (SQLiteException ex) { Console.WriteLine(ex); }
        }
    }
}
