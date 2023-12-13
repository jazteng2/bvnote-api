using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Services;

namespace bvnote_web_api.RouteGroup
{
    public static class BvnGroup
    {
        public static RouteGroupBuilder MapBible(this RouteGroupBuilder group)
        {
            group.MapGet("/books", async (IBookService bookService) =>
            {
                var books = await bookService.GetBooksAsync();
                return books is null ? Results.NotFound("Books does not exist") : Results.Ok(books);
            });

            group.MapGet("/books/{bookId}", async (string bookId, IBookService bookService) =>
            {
                var book = await bookService.GetBookAsync(bookId);
                return book is null ? Results.NotFound("Book does not exist") : Results.Ok(book);
            });

            group.MapGet("/books/{bookId}/verses", async (string bookId, [FromQuery] int chapterNo, IBookService bookService) =>
            {
                var verses = await bookService.GetChapterVerses(bookId, chapterNo);
                return verses is null ? Results.NotFound("Verses does not exist") : Results.Ok(verses);
            });

            group.MapGet("/books/verses", async ([FromQuery] string abbrev, [FromQuery] int chapterNo, IBookService bookService) =>
            {
                var verses = await bookService.GetChapterVerses_abbrev(abbrev, chapterNo);
                return verses is null ? Results.NotFound("Verses does not exist") : Results.Ok(verses);
            });

            group.MapGet("/books/abbreviations", async (IAbbrevService abbrevService) =>
            {
                var abbrevs = await abbrevService.GetAbbrevsAsync();
                return abbrevs is null ? Results.NotFound("Verses does not exist") : Results.Ok(abbrevs);
            });

            return group;
        }
    }
}
