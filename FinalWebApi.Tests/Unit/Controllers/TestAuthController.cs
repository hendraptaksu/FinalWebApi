using FinalWebApi.API.Controllers;
using FinalWebApi.API.Services.BookService;
using FinalWebApi.API.Models;
using FinalWebApi.API.Util;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FinalWebApi.API.DTOs.Responses;
using FinalWebApi.API.DTOs.Requests;
using FinalWebApi.API.Services.TokenService;
using FinalWebApi.API.Services.UserService;
using FinalWebApi.API.Services.HashingService;


namespace FinalWebApi.Tests.Unit.Controllers
{
    public class TestAuthController
    {
        [Fact]
        public void Login_Returns200WhenCredentialIsCorrect()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(s => s.IssueToken("u1")).Returns("jwt_token_example");

            var mockUserService = new Mock<IUserService>();
            // Assume user is exist in DB
            mockUserService.Setup(s => s.GetUser("john@test.com")).Returns(new UserModel(){
                id = 1,
                username = "u1",
                email = "john@test.com",
                password_hash = "hashedpassword",
            });

            var mockPasswordHasher = new Mock<IPasswordHashingService>();
            // Assume supplied password is correct
            mockPasswordHasher.Setup(s => s.Verify("plainpassword", "hashedpassword")).Returns(true);

            var sut = new AuthController(mockTokenService.Object, mockUserService.Object, mockPasswordHasher.Object);

            // Act
            var actionResult = sut.Login(new LoginRequest(){email = "john@test.com", password="plainpassword"});

            var result = actionResult.Result as OkObjectResult;
            var val = result!.Value as BaseResponse<string>;

            // Assert
            result.StatusCode.Should().Be(200);
            val.Data.Should().Be("jwt_token_example");
            val.Message.Should().Be("Success");

            // verify that services are called once
            mockTokenService.Verify(s => s.IssueToken("u1"), Times.Once());
            mockUserService.Verify(s => s.GetUser("john@test.com"), Times.Once());
            mockPasswordHasher.Verify(s => s.Verify("plainpassword", "hashedpassword"), Times.Once());
        }

        [Fact]
        public void Login_Returns400WhenCredentialIsNotCorrect()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenService>(); // just instantiated here because it is used as dependency

            var mockUserService = new Mock<IUserService>();
            // Assume user is exist in DB
            mockUserService.Setup(s => s.GetUser("john@test.com")).Returns(new UserModel(){
                id = 1,
                username = "u1",
                email = "john@test.com",
                password_hash = "hashedpassword",
            });

            var mockPasswordHasher = new Mock<IPasswordHashingService>();
            // Assume supplied password is not correct
            mockPasswordHasher.Setup(s => s.Verify("plainpassword", "hashedpassword")).Returns(false);

            var sut = new AuthController(mockTokenService.Object, mockUserService.Object, mockPasswordHasher.Object);

            // Act
            var actionResult = sut.Login(new LoginRequest(){email = "john@test.com", password="plainpassword"});

            var result = actionResult.Result as BadRequestObjectResult;
            var val = result!.Value as BaseResponse<string>;

            // Assert
            result.StatusCode.Should().Be(400);
            val.Data.Should().BeNull();
            val.Message.Should().Be("Invalid credential");

            // verify that services are called once
            mockUserService.Verify(s => s.GetUser("john@test.com"), Times.Once());
            mockPasswordHasher.Verify(s => s.Verify("plainpassword", "hashedpassword"), Times.Once());
        }

        [Fact]
        public void Signup_Returns201WhenSignupSuccess()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenService>();

            var mockUserService = new Mock<IUserService>();
            // Simulate a success record creation
            mockUserService.Setup(s => s.CreateUser(It.IsAny<UserModel>())).Returns(new UserModel(){
                id = 1,
                username = "u1",
                email = "john@test.com",
                password_hash = "hashedpassword",
            });

            var mockPasswordHasher = new Mock<IPasswordHashingService>();
            mockPasswordHasher.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hashedpassword");

            var sut = new AuthController(mockTokenService.Object, mockUserService.Object, mockPasswordHasher.Object);

            // Act
            var actionResult = sut.Signup(new SignupRequest());

            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<string>;

            // Assert
            result.StatusCode.Should().Be(201);
            val.Data.Should().BeNull();
            val.Message.Should().Be("Success");

            // verify that services are called once
            mockUserService.Verify(s => s.CreateUser(It.IsAny<UserModel>()), Times.Once());
            mockPasswordHasher.Verify(s => s.HashPassword(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Signup_Returns500WhenServiceThrowsException()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenService>();

            var mockUserService = new Mock<IUserService>();
            // Simulate a failed record creation
            mockUserService.Setup(s => s.CreateUser(It.IsAny<UserModel>())).Throws<Exception>();

            var mockPasswordHasher = new Mock<IPasswordHashingService>();
            mockPasswordHasher.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hashedpassword");

            var sut = new AuthController(mockTokenService.Object, mockUserService.Object, mockPasswordHasher.Object);

            // Act
            var actionResult = sut.Signup(new SignupRequest());

            var result = actionResult.Result as ObjectResult;
            var val = result!.Value as BaseResponse<string>;

            // Assert
            result.StatusCode.Should().Be(500);
            val.Data.Should().BeNull();
            val.Message.Should().NotBe("Success");

            // verify that services are called once
            mockUserService.Verify(s => s.CreateUser(It.IsAny<UserModel>()), Times.Once());
            mockPasswordHasher.Verify(s => s.HashPassword(It.IsAny<string>()), Times.Once());
        }
    }
}