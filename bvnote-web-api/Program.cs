using bvnote_web_api.Data;
using Microsoft.AspNetCore.Mvc;
using bvnote_web_api.Data.DTO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WebClients",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500");
        });
});
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<BvnV1Context>(options => options.UseMySQL(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("WebClients");

// API Endpoints
app.MapControllers();
app.MapGet("/api/v1/books", async () =>
{
    var db = new BvnV1Context();
    var dbBooks = await db.Books.ToListAsync();
    
    return dbBooks.Count() > 0 && dbBooks is not null
        ? Results.Ok(BookDTO.GetBookDTOs(dbBooks))
        : Results.NotFound("Unable to fetch books");
});

app.MapGet("/api/v1/books/{bookId}", async (string bookId) =>
{
    var db = new BvnV1Context();
    var book = await db.Books
        .Where(b => b.BookId == bookId)
        .FirstOrDefaultAsync();

    return book is not null
        ? Results.Ok(BookDTO.GetBookDTO(book))
        : Results.NotFound("Unable to fetch book with Id " + bookId);
});

app.MapGet("/api/v1/books/{bookId}/verses", async (string bookId, [FromQuery] int chapterNo) =>
{
    var db = new BvnV1Context();
    var book = await db.Books.FindAsync(bookId);

    if (book is null)
    {
        return Results.NotFound("Book with Id " + bookId + " does not exists");
    }

    var verses = await db.Verses
        .Where(v => v.ChapterNo == chapterNo && v.BookId == bookId)
        .OrderBy(v => v.VerseNo)
        .ToListAsync();

    return verses.Count() > 0 && verses is not null
        ? Results.Ok(VerseDTO.GetVerseDTOs(verses))
        : Results.NotFound("Unable to fetch verses from " + book.Title + " chapter " + chapterNo);
});

app.MapGet("/api/v1/books/abbreviations", async () =>
{
    var db = new BvnV1Context();

    var abbrevs = await db.Abbrevs.ToListAsync();
    return abbrevs.Count() > 0 && abbrevs is not null
        ? Results.Ok(AbbrevDTO.GetAbbrevDTOs(abbrevs))
        : Results.NotFound("Unable to fetch abbreviations");

});
app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });
app.Run();

