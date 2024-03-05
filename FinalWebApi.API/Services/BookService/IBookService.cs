
using FinalWebApi.API.Util;
using FinalWebApi.API.Models;

namespace FinalWebApi.API.Services.BookService;
    public interface IBookService
    {
        BookModel CreateBook(BookModel book);

        List<BookModel> GetBookList();

        BookModel? GetBook(long id);

        ErrorOr<BookModel> UpdateBook(BookModel book);

        bool DeleteBook(long id);
    }