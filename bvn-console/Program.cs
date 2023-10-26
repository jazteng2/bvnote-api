using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;
using bvn_console.Model;
using System.Security.Cryptography;
using System.Data.Entity;
using bvn_console.Data;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Tls;
using System.Data.Entity.ModelConfiguration.Configuration;
using Microsoft.SqlServer.Server;
using ZstdSharp.Unsafe;
using Org.BouncyCastle.Bcpg;

namespace bvn_console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*Console.WriteLine("HABAKKUK 1 1");
            AbbrevAlgorithm("hab 1 1");
            Console.WriteLine("GENESIS 1 1");
            AbbrevAlgorithm("gen 1 1");*/

            foreach (int i in Enumerable.Range(0, 11))
            {
                Console.WriteLine(Guid.NewGuid().ToString());
            }
        }        

        public static void TestDB()
        {
            SQLiteDB db = new SQLiteDB();

            // test bookContext
            BookContext bookContext = new BookContext(db);
            Book book = bookContext.GetById("BOOK000039");

            // test abbrevCotext
            AbbrevContext abbrev = new AbbrevContext(db, bookContext);
            ICollection<Abbreviation> result = abbrev.GetAll();
            foreach (Abbreviation result1 in result)
            {
                Console.WriteLine(result1.Name);
            }

            // test verseContext
            VerseContext verseContext = new VerseContext(db, bookContext);
            List<Verse> verses = verseContext.GetChapterVerses(book.Id, 1);
            foreach (Verse verse in verses)
            {
                Console.WriteLine(verse.VerseNo);
            }
        }

        public static void PopulateSQLiteDB()
        {
            SQLiteDB db = new SQLiteDB();
            BookContext bookContext = new BookContext(db);
            VerseContext verseContext = new VerseContext(db, bookContext);
            List<Book> books = bookContext.GetAll();
            List<Verse> verses = new List<Verse>();

            StreamReader reader = new StreamReader(@"C:\\Users\\jared\\source\\repos\\bvnote-api\\bvn-console\\resources\\NKJV_comma.txt");
            string line = reader.ReadLine();
            string[] rst = line.Split(",");

            // FIRST VERSE
            string id = "VERSE00001";
            int count = 1;
            /*verses.Add(new Verse()
            {
                Id = id,
                ChapterNo = Convert.ToInt32(rst[1]),
                VerseNo = Convert.ToInt32(rst[2]),
                Content = rst[3],
                BookID = books[Convert.ToInt32(rst[0]) - 1].Id
            });
            count++;*/

            // ADD OTHER VERSES
            while (line != null)
            {
                int digits = (int) Math.Floor(Math.Log10(count) + 1);
                id = id[0..(id.Length - digits)] + count.ToString();
                rst = line.Split(",");
                verses.Add(new Verse()
                {
                    Id = id,
                    ChapterNo = Convert.ToInt32(rst[1]),
                    VerseNo = Convert.ToInt32(rst[2]),
                    Content = rst[3],
                    BookID = books[Convert.ToInt32(rst[0]) - 1].Id
                });
                count++;
                line = reader.ReadLine();
            }
            reader.Close();
            verseContext.AddVerses(verses);
        }

        public static void TxtTabtoComma()
        {
            StreamReader reader = new StreamReader(@"C:\\Users\\jared\\source\\repos\\bvnote-api\\bvn-console\\resources\\NKJV_fixed.txt");
            List<string> lines = new();
            try
            {
                string line = reader.ReadLine();
                lines.Add(line);
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { reader.Close(); }

            // Replace and write into new txt
            StreamWriter writer = new StreamWriter(@"C:\\Users\\jared\\source\\repos\\bvnote-api\\bvn-console\\resources\\NKJV_comma.txt");
            foreach (string l in lines)
            {
                string newLine = l.Replace('\t', ',');
                writer.WriteLine(newLine);
            }
        }

        public static void AbbrevAlgorithm(string input)
        {
            /// input --> "[Book] [chapter] [verse]"
            /// substrings --> input.ToLower().Trim().Split(" ")
            /// List Abbrevs --> abbrevContext.GetAll()
            /// loop (abbrev.Name.ToLower() == substring)
            /// BookInfo --> bookContext.GetByName(bookName)
            /// ChapterVerses --> verseContext.GetChapterVerses(chapterNo)
            /// Verse --> loop (ChapterVerses[index].VerseNo == substring.verse.VerseNo )
            /// Display

            SQLiteDB db = new SQLiteDB();
            BookContext bookContext = new BookContext(db);
            AbbrevContext abbrevContext = new AbbrevContext(db, bookContext);
            VerseContext verseContext = new VerseContext(db, bookContext);
            List<Abbreviation> abbrevs = abbrevContext.GetAll();

            // Get Book based on abbrev
            string[] subInputs = input.ToLower().Trim().Split(' ');
            Book book = new Book();
            foreach (Abbreviation a in abbrevs)
            {
                if (a.Name.ToLower().Equals(subInputs[0]))
                {
                    book = a.Book;
                    break;
                }
            }

            // Get chapter verses
            Console.WriteLine("INPUT GET CHAPTER VERSES: " + book.Id + " " + subInputs[1]);
            List<Verse> verses = verseContext.GetChapterVerses(book.Id, Convert.ToInt16(subInputs[1]));
            foreach (Verse verse in verses)
            {
                Console.WriteLine(verse.ChapterNo + " " + verse.VerseNo + " " + verse.Content);
            }
        }
    }
}