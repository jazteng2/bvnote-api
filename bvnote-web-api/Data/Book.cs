using System;
using System.Collections.Generic;

namespace bvnote_web_api.Data;

public partial class Book
{
    public string BookId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public ulong OldTestament { get; set; }

    public virtual ICollection<Abbrev> Abbrevs { get; set; } = new List<Abbrev>();

    public virtual ICollection<Verse> Verses { get; set; } = new List<Verse>();
}
