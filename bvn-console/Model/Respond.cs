using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvn_console.Model
{
    public class Respond
    {
        public string? Book { get; set; }
        public string? Chapter { get; set; }
        public int pickedVerse { get; set; }
        public List<Verse> Verses { get; set; } = new List<Verse>();
    }
}
