using FinalWebApi.API.Models;
using FinalWebApi.API.Database;
using FinalWebApi.API.Util;

namespace FinalWebApi.API.Services.BookService;

public class BookService : IBookService
{
    private readonly AppDbContext dbContext;

    public BookService(AppDbContext c)
    {
        dbContext = c;
    }

    public BookModel CreateBook(BookModel book)
    {
        dbContext.Books.Add(book);
        dbContext.SaveChanges();

        return book;
    }

    public bool DeleteBook(long id)
    {
        var bookToDelete = dbContext.Books.Where(b => b.id == id).FirstOrDefault();

        if (bookToDelete == null)
        {
            return false;
        }

        dbContext.Books.Remove(bookToDelete);
        dbContext.SaveChanges();

        return true;
    }

    public ErrorOr<BookModel> UpdateBook(BookModel data) {
        ErrorOr<BookModel> result = new ();
        var existingBook = dbContext.Books.Where(b => b.id == data.id).FirstOrDefault();

        if (existingBook == null) {
            return result.AddError(new NotFoundError("Cannot find book"));
        }

        existingBook.title = data.title;
        existingBook.author = data.author;
        existingBook.publication_year =data.publication_year;
        existingBook.page_count = data.page_count;

        dbContext.SaveChanges();

        return result.AddValue(existingBook);
    }

    public BookModel? GetBook(long id)
    {
        return dbContext.Books.Where(b => b.id == id).FirstOrDefault();
    }

    public List<BookModel> GetBookList()
    {
        return dbContext.Books.ToList();
    }
}
