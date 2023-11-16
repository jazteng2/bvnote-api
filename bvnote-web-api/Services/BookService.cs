using bvnote_web_api.Data;
using bvnote_web_api.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace bvnote_web_api.Services
{
    public class BookService
    {
        private readonly DbBvnContext _db;
        private readonly IMemoryCache _memoryCache;

        public BookService(DbBvnContext db, IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        public async Task<List<BookDTO>> GetBooksAsync()
        {
            // get cache value
            var cacheKey = "books";
            if (_memoryCache.TryGetValue(cacheKey, out List<BookDTO>? cacheValue))
                return cacheValue is null
                    ? new List<BookDTO>()
                    : cacheValue;

            // get fresh data from db
            var books = await _db.Books.ToListAsync();
            if (books is null) return new List<BookDTO>();

            // set new cache memory
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.Low);

            _memoryCache.Set(cacheKey, books, cacheEntryOptions);
            return BookDTO.GetBookDTOs(books);
        }

        public async Task<BookDTO> GetBookAsync(string bookId)
        {
            // get cache value
            var cacheKey = "book_" + bookId;
            if (_memoryCache.TryGetValue(cacheKey, out BookDTO? cacheValue))
                return cacheValue is null
                    ? new BookDTO()
                    : cacheValue;

            // get fresh data from db
            var book = await _db.Books.Where(b => b.BookId == bookId).FirstOrDefaultAsync();
            if (book is null) return new BookDTO();

            // set new cache memory
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(3))
                            .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cacheKey, book, cacheEntryOptions);
            return BookDTO.GetBookDTO(book);
        }

        public async Task<List<VerseDTO>> GetChapterVerses(string bookId, int chapterNo)
        {
            // get cache value
            var cacheKey = "verses_" + bookId + "_" + chapterNo;
            if (_memoryCache.TryGetValue(cacheKey,out List<VerseDTO>? cacheValue)) 
                return cacheValue is null
                    ? new List<VerseDTO>() 
                    : cacheValue;

            // get fresh data from db
            var verses = await _db.Verses
                    .Where(v => v.ChapterNo == chapterNo && v.BookId == bookId)
                    .OrderBy(v => v.VerseNo)
                    .ToListAsync();
            if (verses is null) return new List<VerseDTO>();

            // set new cache memory
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                            .SetPriority(CacheItemPriority.High);
            _memoryCache.Set(cacheKey, verses, cacheEntryOptions);
            return VerseDTO.GetVerseDTOs(verses);
        }

        public async Task<List<VerseDTO>> GetChapterVerses_abbrev(string bookAbbrev, int chapterNo)
        {
            var abbrev = await _db.Abbrevs
                .Where(a => a.Abbreviation.ToLower().Equals(bookAbbrev.ToLower()))
                .FirstOrDefaultAsync();
            return abbrev is null 
                ? new List<VerseDTO>() 
                : await GetChapterVerses(abbrev.BookId, chapterNo);
        }
    }
}
