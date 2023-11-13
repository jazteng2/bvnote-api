using System;
using System.Collections.Generic;

namespace bvnote_web_api.Data;

public partial class Abbrev
{
    public string AbbrevId { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public string BookId { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
