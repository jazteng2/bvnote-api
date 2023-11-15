using System;
using System.Collections.Generic;

namespace bvnote_web_api.Data;

public partial class Verse
{
    public Guid VerseId { get; set; }

    public int? ChapterNo { get; set; }

    public int? VerseNo { get; set; }

    public string Content { get; set; } = null!;

    public string BookId { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
