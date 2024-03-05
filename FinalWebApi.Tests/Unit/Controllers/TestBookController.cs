using FinalWebApi.API.Controllers;
using FinalWebApi.API.Services.BookService;
using FinalWebApi.API.Models;
using FinalWebApi.API.Util;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FinalWebApi.API.DTOs.Responses;
using FinalWebApi.API.DTOs.Requests;


namespace FinalWebApi.Tests.Unit.Controllers
{
    public class TestBookController
    {
        [Fact]
        public void CreateBook_Returns201WhenSuccessSavingToDB()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.CreateBook(It.IsAny<BookModel>())).Returns(new BookModel()
            {
                id = 1,
                title = "t1",
                author = "a1",
                publication_year = 2020,
                page_count = 10,
            });

            var sut = new BookController(mockBookService.Object);

            // Act
            var actionResult = sut.CreateBook(new CreateBookRequest());

            // In order to know, what the actual object holds in actionResult.Result, 
            // we need to run the test in debug mode and 
            // place breakpoint at this line. Which IMO very painful.
            // Once the actual type is known, typecast to that type with `as WhateverIsTheActualObjectType`.
            // Alternatively, use Console.WriteLine() to get the object type.
            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(201);
            val.Data.id.Should().Be(1);
            val.Data.author.Should().Be("a1");

            // verify that service is called once
            mockBookService.Verify(s => s.CreateBook(It.IsAny<BookModel>()), Times.Once());
        }

        [Fact]
        public void CreateBook_Returns500WhenServiceThrowsException()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.CreateBook(It.IsAny<BookModel>())).Throws<Exception>();

            var sut = new BookController(mockBookService.Object);

            // Act
            var actionResult = sut.CreateBook(new CreateBookRequest());

            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(500);
            val.Message.Should().NotBe("Success");
            val.Data.Should().BeNull();

            // verify that service is called once
            mockBookService.Verify(s => s.CreateBook(It.IsAny<BookModel>()), Times.Once());
        }

        [Fact]
        public void GetBookList_Returns200WhenServiceReturnSuccess()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.GetBookList()).Returns(new List<BookModel> {
                new() {
                    id = 1,
                    title ="book1",
                    author="author1",
                    publication_year=2000,
                    page_count=200,
                },
                new() {
                    id = 2,
                    title ="book2",
                    author="author2",
                    publication_year=1000,
                    page_count=100,
                }
            });

            var sut = new BookController(mockBookService.Object);

            // Act
            var actionResult = sut.GetBookList();

            // This typecasting is painful right? 
            var result = actionResult.Result as OkObjectResult;
            var val = result!.Value as BaseResponse<List<BookModel>>;

            // Assert 
            result.StatusCode.Should().Be(200);
            val!.Data.Count.Should().Be(2);
            val!.Message.Should().Be("Success");

            // assert that the service is called once
            mockBookService.Verify(s => s.GetBookList(), Times.Once());
        }

        [Fact]
        public void GetBookList_Returns500WhenServiceThrowException()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.GetBookList()).Throws<Exception>();

            var sut = new BookController(mockBookService.Object);

            // Act
            var actionResult = sut.GetBookList();

            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<List<BookModel>>;

            // Assert 
            // assert api response body structure
            result.StatusCode.Should().Be(500);
            val!.Data.Should().BeNull();
            val!.Message.Should().NotBe("Success");

            // assert that the service is called once
            mockBookService.Verify(s => s.GetBookList(), Times.Once());
        }

        [Fact]
        public void GetBook_Returns200WhenRecordExists()
        {
            //Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.GetBook(1)).Returns(new BookModel
            {
                id = 1,
                title = "book1",
                author = "author1",
                publication_year = 2000,
                page_count = 200,
            });

            var sut = new BookController(mockBookService.Object);
            //Act
            var actionResult = sut.GetBook(1);

            var result = actionResult.Result as OkObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            //Assert
            result.StatusCode.Should().Be(200);
            val!.Data.id.Should().Be(1);
            val!.Data.title.Should().Be("book1");
            val!.Message.Should().Be("Success");

            // assert that the service is called once
            mockBookService.Verify(s => s.GetBook(1), Times.Once());
        }

        [Fact]
        public void GetBook_Returns404WhenRecordDoesNotExists()
        {
            //Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.GetBook(1)).Returns(null as BookModel);

            var sut = new BookController(mockBookService.Object);
            //Act
            var actionResult = sut.GetBook(1);

            var result = actionResult.Result as NotFoundObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            //Assert
            result.StatusCode.Should().Be(404);
            val!.Data.Should().BeNull();
            val!.Message.Should().Be("Not Found");

            // assert that the service is called exactly once
            mockBookService.Verify(s => s.GetBook(1), Times.Once());
        }

        [Fact]
        public void UpdateBook_Returns200WhenSuccessUpdatesRecord()
        {
            // Arrange
            var bookService = new Mock<IBookService>();
            // set mock service to return success update result
            bookService
                .Setup(s => s.UpdateBook(It.IsAny<BookModel>()))
                .Returns(
                    new ErrorOr<BookModel>().AddValue(new BookModel
                    {
                        id = 1,
                        title = "book1",
                        author = "author1",
                        publication_year = 2000,
                        page_count = 200,
                    })
                );

            var sut = new BookController(bookService.Object);

            // Act
            var actionResult = sut.UpdateBook(1, new UpdateBookRequest());

            var result = actionResult.Result as OkObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(200);
            val.Data.id.Should().Be(1);
            val.Data.author.Should().Be("author1");

            // verify that service is called once
            bookService.Verify(s => s.UpdateBook(It.IsAny<BookModel>()), Times.Once());
        }

        [Fact]
        public void UpdateBook_Returns404WhenRecordIdDoesNotExist()
        {
            // Arrange
            var bookService = new Mock<IBookService>();
            bookService
                .Setup(s => s.UpdateBook(It.IsAny<BookModel>()))
                .Returns(
                    new ErrorOr<BookModel>().AddError(new NotFoundError())
                );

            var sut = new BookController(bookService.Object);

            // Act
            var actionResult = sut.UpdateBook(1, new UpdateBookRequest());

            var result = actionResult.Result as NotFoundObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(404);
            val.Data.Should().BeNull();
            val.Message.Should().Be("Not Found");

            // verify that service is called once
            bookService.Verify(s => s.UpdateBook(It.IsAny<BookModel>()), Times.Once());
        }

        [Fact]
        public void DeleteBook_Returns200WhenDeleteSuccess()
        {
            // Arrange
            var bookService = new Mock<IBookService>();
            bookService.Setup(s => s.DeleteBook(1)).Returns(true);

            var sut = new BookController(bookService.Object);

            // Act
            var actionResult = sut.DeleteBook(1);

            var result = actionResult.Result as OkObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(200);
            val.Data.Should().BeNull();
            val.Message.Should().Be("Success");

            // verify that service is called once
            bookService.Verify(s => s.DeleteBook(1), Times.Once());
        }

        [Fact]
        public void DeleteBook_Returns404WhenRecordDoesNotExists()
        {
            // Arrange
            var bookService = new Mock<IBookService>();
            bookService.Setup(s => s.DeleteBook(1)).Returns(false);

            var sut = new BookController(bookService.Object);

            // Act
            var actionResult = sut.DeleteBook(1);

            var result = actionResult.Result as NotFoundObjectResult;
            var val = result!.Value as BaseResponse<BookModel>;

            // Assert
            result.StatusCode.Should().Be(404);
            val.Data.Should().BeNull();
            val.Message.Should().Be("Not Found");

            // verify that service is called once
            bookService.Verify(s => s.DeleteBook(1), Times.Once());
        }
    }
}