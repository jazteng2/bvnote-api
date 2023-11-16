using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Services;

namespace bvnote_web_api.RouteGroup
{
    public static class BibleGroup
    {
        public static RouteGroupBuilder MapBible(this RouteGroupBuilder group)
        {
            group.MapGet("/books", async (BookService bookService)
                => Results.Ok(await bookService.GetBooksAsync()));

            group.MapGet("/books/{bookId:length(10)}", async (string bookId, BookService bookService)
                => Results.Ok(await bookService.GetBookAsync(bookId)));

            group.MapGet("/books/{bookId:length(10)}/verses", async (string bookId, [FromQuery] int chapterNo, BookService bookService)
                => Results.Ok(await bookService.GetChapterVerses(bookId, chapterNo)));

            group.MapGet("/books/verses", async ([FromQuery] string abbrev, [FromQuery] int chapterNo, BookService bookService)
                => Results.Ok(await bookService.GetChapterVerses_abbrev(abbrev, chapterNo)));

            return group;
        }

        public static RouteGroupBuilder MapDocuments(this RouteGroupBuilder group)
        {
            group.MapGet("/documents/{id}", () =>
            {

            });
            return group;
        }
    }
}
