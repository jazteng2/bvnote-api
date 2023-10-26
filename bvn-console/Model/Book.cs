using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvn_console.Model
{
    public class Book
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public bool OldTestament { get; set; }
    }
}
