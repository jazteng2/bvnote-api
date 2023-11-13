namespace bvnote_web_api.Data.DTO
{
    public record BookDTO
    {
        public string? BookID { get; set; }
        public string? Title { get; set; }
        public bool OldTestament {  get; set; }

        // TODO: apply asynchronous
        public static BookDTO GetBookDTO(Book book)
        {
            return new BookDTO
            {
                BookID = book.BookId, Title = book.Title, OldTestament = book.OldTestament
            };
        }

        // TODO: apply asynchronous
        public static List<BookDTO> GetBookDTOs(List<Book> books)
        {
            var bookDTOs = new List<BookDTO>();
            foreach (var book in books)
            {
                bookDTOs.Add(new BookDTO
                {
                    BookID = book.BookId,
                    Title = book.Title,
                    OldTestament = book.OldTestament
                });
            }

            return bookDTOs;
        }
    }
}
