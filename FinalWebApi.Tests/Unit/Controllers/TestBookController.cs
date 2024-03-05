using FinalWebApi.API.Controllers;
using FinalWebApi.API.Services.BookService;
using FinalWebApi.API.Models;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FinalWebApi.API.DTOs.Responses;

namespace FinalWebApi.Tests.Unit.Controllers
{
    public class TestBookController
    {
        [Fact]
        public void GetBookList_ReturnsSuccessWhenServiceReturnSuccess()
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
            // assert api response body structure
            val!.Data.Count.Should().Be(2);
            val!.Message.Should().Be("Success");

            // assert that the service is called once
            mockBookService.Verify(s => s.GetBookList(), Times.AtMostOnce());
        }

        [Fact]
        public void GetBookList_ReturnsErrorWhenServiceThrowException()
        {
            // Arrange
            var mockBookService = new Mock<IBookService>();
            mockBookService.Setup(s => s.GetBookList()).Throws<Exception>();

            var sut = new BookController(mockBookService.Object);

            // Act
            var actionResult = sut.GetBookList();

            // In order to know, what the actual object holds in actionResult.Result, 
            // you need to run the test in debug mode and 
            // place breakpoint at this line. Which IMO very painful.
            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<List<BookModel>>;

            // Assert 
            // assert api response body structure
            val!.Data.Should().BeNull();
            val!.Message.Should().NotBe("Success");

            // assert that the service is called once
            mockBookService.Verify(s => s.GetBookList(), Times.AtMostOnce());
        }
    }
}