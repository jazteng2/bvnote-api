using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvn_console.Model
{
    public class Verse
    {
        public string Id { get; set; } = null!;
        public int ChapterNo { get; set; }
        public int VerseNo { get; set; }
        public string Content { get; set; } = null!;
        public string BookID { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
