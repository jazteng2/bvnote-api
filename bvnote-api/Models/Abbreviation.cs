using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bvnote_api.Models
{
    public class Abbreviation
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string BookID { get; set; } = null!;
    }
}
