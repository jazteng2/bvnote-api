using System.Collections.Concurrent;

namespace bvnote_web_api.Data.DTO
{
    public record VerseDTO
    {
        public Guid VerseId { get; set; }
        public int? ChapterNo { get; set; }
        public int? VerseNo { get; set; }
        public string? Content { get; set; }

        // TODO: apply asynchronous
        public static List<VerseDTO> GetVerseDTOs(List<Verse> verses)
        {
            return verses.Select(verse => new VerseDTO
            {
                VerseId = verse.VerseId,
                ChapterNo = verse.ChapterNo,
                VerseNo = verse.VerseNo,
                Content = verse.Content
            }).ToList();
        }

        public static List<VerseDTO> GetVerseDTOs_Parallel(List<Verse> verses)
        {
            var verseDTOs = new ConcurrentBag<VerseDTO>();
            Parallel.ForEach(verses, verse =>
            {
                verseDTOs.Add(new VerseDTO
                {
                    VerseId = verse.VerseId,
                    ChapterNo = verse.ChapterNo,
                    VerseNo = verse.VerseNo,
                    Content = verse.Content
                });
            });
            return verseDTOs.ToList();
        }
    }
}
