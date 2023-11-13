namespace bvnote_web_api.Data.DTO
{
    public record VerseDTO
    {
        public string? VerseId { get; set; }
        public int? ChapterNo { get; set; }
        public int? VerseNo { get; set; }
        public string? Content { get; set; }

        // TODO: apply asynchronous
        public static List<VerseDTO> GetVerseDTOs(List<Verse> verses)
        {
            var verseDTOs = new List<VerseDTO>();
            foreach (var verse in verses)
            {
                verseDTOs.Add(new VerseDTO
                {
                    VerseId = verse.VerseId,
                    ChapterNo = verse.ChapterNo,
                    VerseNo = verse.VerseNo,
                    Content = verse.Content
                });
            }
            return verseDTOs;
        }
    }
}
