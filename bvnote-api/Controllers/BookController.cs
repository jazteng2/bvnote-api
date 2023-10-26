using Microsoft.AspNetCore.Mvc;
using bvnote_api.Data;
using bvnote_api.Models;
using MySqlX.XDevAPI.Common;

namespace bvnote_api.Controllers
{
    [Route("api/books")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BookController : ControllerBase
    {

        private readonly BookContext _bookContext;
        private readonly VerseContext _verseContext;
        private readonly AbbrevContext _abbrevContext;

        // Constructor for verse controller
        public BookController(DbContext dbContext)
        {
            _bookContext = new BookContext(dbContext);
            _verseContext = new VerseContext(dbContext);
            _abbrevContext = new AbbrevContext(dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooksAsync()
        {
            List<Book> books = await _bookContext.GetListAsync();
            return books is null ? NotFound() : Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookAsync(string id)
        {
            Book book = await _bookContext.GetByIdAsync(id);
            return book is null ? NotFound() : Ok(book);
        }

        [HttpGet("{id}/verses")]
        public async Task<IActionResult> GetChapterVersesAsync(string id, [FromQuery] int chapterNo)
        {
            Book book = await _bookContext.GetByIdAsync(id);
            List<Verse> verses = await _verseContext.GetChapterVersesAsync(book, chapterNo);
            return book is null && verses is null ? NotFound() : Ok(verses);
        }

        [HttpGet("abbreviations")]
        public async Task<IActionResult> GetAbbreviationsAsync()
        {
            List<Abbreviation> abbreviations = await _abbrevContext.GetAllAsync();
            return abbreviations is null ? NotFound() : Ok(abbreviations);
        }

        [HttpPost]
        public IActionResult PostBook(Book book)
        {
            _bookContext.Add(book);
            return _bookContext.GetById(book.Id).Id.Equals(book.Id)
                ? Ok() 
                : BadRequest("PostBook Unsuccessful");
        }
    }
}
