using FinalWebApi.API.DTOs.Requests;
using FinalWebApi.API.DTOs.Responses;
using FinalWebApi.API.Services.BookService;
using Microsoft.AspNetCore.Mvc;
using FinalWebApi.API.Models;
using FinalWebApi.API.Util;

namespace FinalWebApi.API.Controllers;

[Route("api/books")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService bookService;

    public BookController(IBookService bs)
    {
        this.bookService = bs;
    }

    [HttpPost]
    public ActionResult<BaseResponse<BookModel>> CreateBook(CreateBookRequest req)
    {
        try
        {
            var newBook = new BookModel
            {
                title = req.title,
                author = req.author,
                publication_year = req.publication_year,
                page_count = req.page_count,
            };

            var book = bookService.CreateBook(newBook);

            return StatusCode(StatusCodes.Status201Created, new BaseResponse<BookModel>
            {
                Message = "Success",
                Data = book,
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse<BookModel>
            {
                Message = e.Message,
            });
        }
    }

    [HttpGet]
    public ActionResult<BaseResponse<List<BookModel>>> GetBookList()
    {
        try
        {
            var books = bookService.GetBookList();
            return Ok(new BaseResponse<List<BookModel>>
            {
                Message = "Success",
                Data = books,
            });
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new BaseResponse<List<BookModel>>
                {
                    Message = e.Message
                }
            );
        }
    }

    [HttpGet("{id}")]
    public ActionResult<BaseResponse<BookModel?>> GetBook(long id)
    {
        var book = bookService.GetBook(id);
        if (book == null)
        {
            return NotFound(new BaseResponse<BookModel?>
            {
                Message = "Not Found",
            });
        }

        return Ok(new BaseResponse<BookModel?>
        {
            Message = "Success",
            Data = book,
        });
    }

    [HttpPut("{id}")]
    public ActionResult<BaseResponse<BookModel>> UpdateBook(long id, [FromBody] UpdateBookRequest req)
    {
        var bookData = new BookModel
        {
            id = id,
            title = req.title,
            author = req.author,
            publication_year = req.publication_year,
            page_count = req.page_count,
        };

        var result = bookService.UpdateBook(bookData);
        if (result.HasError)
        {
            if (result.Error is NotFoundError)
            {
                return NotFound(new BaseResponse<BookModel>()
                {
                    Message = result.Error.Message,
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse<BookModel>()
            {
                Message = result.Error.Message,
            });
        }

        return Ok(new BaseResponse<BookModel>()
        {
            Message = "Success",
            Data = result.Value,
        });
    }

    [HttpDelete("{id}")]
    public ActionResult<BaseResponse<BookModel>> DeleteBook(long id)
    {

        var success = bookService.DeleteBook(id);
        if (!success)
        {
            return NotFound(new BaseResponse<BookModel>
            {
                Message = "Not Found"
            });
        }

        return Ok(new BaseResponse<BookModel>
        {
            Message = "Success"
        });
    }
}
