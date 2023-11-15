using bvnote_web_api.Data;
using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

                return books is not null
                    ? Results.Ok(books)
                    : Results.NotFound("Unable to fetch books");
            });


            group.MapGet("/books/{bookId}", async (string bookId, BookService bookService) =>
            {
                var book = bookService.GetBookAsync(bookId);

                return book is not null
                    ? Results.Ok(book)
                    : Results.NotFound("Unable to fetch book with Id " + bookId);
            });

            /*group.MapGet("/books/{bookId}/verses", async (string bookId, [FromQuery] int chapterNo, BvnV1Context db) =>
            {
                var book = await db.Books.FindAsync(bookId);

                if (book is null)
                {
                    return Results.NotFound($"Book with Id {bookId} does not exists");
                }

                var verses = await db.Verses
                    .Where(v => v.ChapterNo == chapterNo && v.BookId == bookId)
                    .OrderBy(v => v.VerseNo)
                    .ToListAsync();

                return verses.Count() > 0 && verses is not null
                    ? Results.Ok(VerseDTO.GetVerseDTOs(verses))
                    : Results.NotFound($"Unable to fetch verses from {book.Title} chapter {chapterNo}");
            });

            group.MapGet("/api/v1/books/abbreviations", async (BvnV1Context db) =>
            {
                var abbrevs = await db.Abbrevs.ToListAsync();
                return abbrevs.Count() > 0 && abbrevs is not null
                    ? Results.Ok(AbbrevDTO.GetAbbrevDTOs(abbrevs))
                    : Results.NotFound("Unable to fetch abbreviations");

            });*/

            return group;
        }
    }
}
