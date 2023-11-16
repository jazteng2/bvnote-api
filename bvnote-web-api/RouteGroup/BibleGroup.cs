using bvnote_web_api.Data;
using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Data.DTO;
using bvnote_web_api.Services;

namespace bvnote_web_api.RouteGroup
{
    public static class BibleGroup
    {
        public static RouteGroupBuilder MapBible(this RouteGroupBuilder group)
        {
            group.MapGet("/books", async (BookService bookService) =>
            {
                var books = await bookService.GetBooksAsync();
                return Results.Ok(books);
            });

            group.MapGet("/books/{bookId:length(10)}", async (string bookId, BookService bookService) =>
            {
                var book = await bookService.GetBookAsync(bookId);
                return Results.Ok(book);
            });

            group.MapGet("/books/{bookId:length(10)}/verses", async (string bookId, [FromQuery] int chapterNo, BookService bookService) =>
            {
                var verses = await bookService.GetChapterVerses(bookId, chapterNo);
                return Results.Ok(verses);
            });

            group.MapGet("/books/verses", async ([FromQuery] string abbrev, [FromQuery] int chapterNo, BookService bookService) =>
            {
                var verses = await bookService.GetChapterVerses_abbrev(abbrev, chapterNo);
                return Results.Ok(verses);
            });

            return group;
        }
    }
}
