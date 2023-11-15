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
            // check cache value
            var cacheKey = "books";
            if (_memoryCache.TryGetValue(cacheKey, out List<BookDTO> cacheValue))
            {
                return cacheValue is null
                    ? new List<BookDTO>()
                    : cacheValue;
            }

            // check books
            var books = await _db.Books.ToListAsync();
            if (books is null) 
            {
                return new List<BookDTO>();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cacheKey, books, cacheEntryOptions);

            return BookDTO.GetBookDTOs(books);
        }

        public async Task<BookDTO> GetBookAsync(string bookId)
        {
            // check cache value
            var cacheKey = "book";
            if (_memoryCache.TryGetValue(cacheKey, out BookDTO cacheValue))
            {
                return cacheValue is null
                    ? new BookDTO()
                    : cacheValue;
            }

            var book = await _db.Books
                    .Where(b => b.BookId == bookId)
                    .FirstOrDefaultAsync();
            if (book is null)
            {
                return new BookDTO();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(cacheKey, book, cacheEntryOptions);

            return BookDTO.GetBookDTO(book);
        }
    }
}
