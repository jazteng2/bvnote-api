using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvn_console.Model
{
    public class Abbreviation
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string BookID { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
