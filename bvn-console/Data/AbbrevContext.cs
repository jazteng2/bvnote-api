using bvn_console.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvn_console.Data
{
    public class AbbrevContext
    {
        private readonly SQLiteDB _db;
        private readonly SQLiteConnection _conn;
        private readonly BookContext _bookContext;
        public AbbrevContext(SQLiteDB db, BookContext bookContext)
        {
            _db = db;
            _conn = _db.Connection;
            _bookContext = bookContext;
        }

        public List<Abbreviation> GetAll()
        {
            List<Abbreviation> result = new List<Abbreviation>();
            SQLiteCommand cmd = new SQLiteCommand(_conn);
            cmd.CommandText = "SELECT * FROM Abbrev";
            try
            {
                _conn.Open();
                SQLiteDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    result.Add(new Abbreviation()
                    {
                        Id = rd.GetString(0),
                        Name = rd.GetString(1),
                        BookID = rd.GetString(2),
                    });
                }
                rd.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { _conn.Close(); }

            foreach (Abbreviation a in result)
            {
                a.Book = _bookContext.GetById(a.BookID);
            }

            return result;
        }

        public Abbreviation GetByName(string name)
        {
            Abbreviation result = new Abbreviation();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Abbrev WHERE Abbreviation = @name", _conn);
            cmd.Parameters.AddWithValue("name", name);
            try
            {
                _conn.Open();
                SQLiteDataReader rd = cmd.ExecuteReader();
                result.Id = rd.GetString(0);
                result.Name = rd.GetString(1);
                result.BookID = rd.GetString(2);
                result.Book = _bookContext.GetById(result.BookID);
                rd.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            finally { _conn.Close(); }
            return result;
        }
    }
}
