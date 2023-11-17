using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Services;

namespace bvnote_web_api.RouteGroup
{
    public static class BvnGroup
    {
        public static RouteGroupBuilder MapBible(this RouteGroupBuilder group)
        {
            group.MapGet("/books", async (IBookService bookService)
                => Results.Ok(await bookService.GetBooksAsync()));

            group.MapGet("/books/{bookId}", async (string bookId, IBookService bookService)
                => Results.Ok(await bookService.GetBookAsync(bookId)));

            group.MapGet("/books/{bookId}/verses", async (string bookId, [FromQuery] int chapterNo, IBookService bookService)
                => Results.Ok(await bookService.GetChapterVerses(bookId, chapterNo)));

            group.MapGet("/books/verses", async ([FromQuery] string abbrev, [FromQuery] int chapterNo, IBookService bookService)
                => Results.Ok(await bookService.GetChapterVerses_abbrev(abbrev, chapterNo)));

            return group;
        }

        public static RouteGroupBuilder MapDocuments(this RouteGroupBuilder group)
        {
            group.MapGet("/documents/{id}", () =>
            {

            });

            // Search
            group.MapGet("/documents", ([FromQuery] string find) =>
            {

            });

            group.MapGet("/documents/{id}", (string id, [FromQuery] string find) =>
            {

            });

            // By Verses
            group.MapGet("/verses/{id}/documents", () =>
            {

            });

            group.MapGet("/verses/{id}/documents/{id}", () =>
            {

            });

            // By Videos
            group.MapGet("/videos/{id}/documents", () =>
            {

            });

            group.MapGet("/videos/{id}/documents/{id}", () =>
            {

            });

            return group;
        }
    }
}
