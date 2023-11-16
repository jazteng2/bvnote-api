using bvnote_web_api.Data.DTO;

namespace bvnote_web_api.Services
{
    public interface IBookService
    {
        public Task<List<BookDTO>> GetBooksAsync();
        public Task<BookDTO> GetBookAsync(string bookId);
        public Task<List<VerseDTO>> GetChapterVerses(string bookId, int chapterNo);
        public Task<List<VerseDTO>> GetChapterVerses_abbrev(string bookAbbrev, int chapterNo);
    }
}
