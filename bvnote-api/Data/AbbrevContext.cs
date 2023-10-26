using MySql.Data.MySqlClient;
using bvnote_api.Models;
namespace bvnote_api.Data
{
    public class AbbrevContext
    {
        private readonly MySqlConnection _conn;
        private readonly BookContext _bookContext;
        public AbbrevContext(DbContext dbContext)
        {
            _conn = dbContext.Connection();
            _bookContext = new BookContext(dbContext);
        }

        public List<Abbreviation> GetAll()
        {
            List<Abbreviation> result = new List<Abbreviation>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Abbrev", _conn);
            try
            {
                _conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    result.Add(new Abbreviation()
                    {
                        Id = rd.GetString(0),
                        Name = rd.GetString(1),
                        BookID = rd.GetString(2)
                    });
                }
                rd.Close();
                _conn.Close();
            }
            catch (MySqlException e) { Console.WriteLine(e); }
            return result;
        }

        public async Task<List<Abbreviation>> GetAllAsync()
        {
            List<Abbreviation> result = new List<Abbreviation>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Abbrev", _conn);
            try
            {
                await _conn.OpenAsync();
                var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    result.Add(new Abbreviation()
                    {
                        Id = rd.GetString(0),
                        Name = rd.GetString(1),
                        BookID = rd.GetString(2)
                    });
                }
                await rd.CloseAsync();
            }
            catch (MySqlException e) { Console.WriteLine(e); }
            await _conn.CloseAsync();
            return result;
        }
    }
}
