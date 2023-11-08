using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvnote_api.Models
{
    public class Verse
    {
        public Guid Id { get; set; }
        public int ChapterNo { get; set; }
        public int VerseNo { get; set; }
        public string Content { get; set; } = null!;
        public string BookId { get; set; } = null!;
    }
}
