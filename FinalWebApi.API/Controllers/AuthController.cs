using FinalWebApi.API.DTOs.Requests;
using FinalWebApi.API.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using FinalWebApi.API.Services.TokenService;
using FinalWebApi.API.Services.UserService;
using FinalWebApi.API.Models;
using BC = BCrypt.Net.BCrypt;
using FinalWebApi.API.Services.HashingService;


namespace FinalWebApi.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService tokenService;
    private readonly IUserService userService;
    private readonly IPasswordHashingService passwordHashingService;

    public AuthController(ITokenService ts, IUserService us, IPasswordHashingService phs)
    {
        tokenService = ts;
        userService = us;
        passwordHashingService = phs;
    }

    [HttpPost]
    [Route("login")]
    public ActionResult<BaseResponse<string>> Login(LoginRequest req)
    {
        var user = userService.GetUser(req.email);
        if (user == null) {
            return BadRequest(new BaseResponse<string>{
                Message = "Invalid credential",
            });
        }

        // compare plain and hashed password, if not matched return bad request
        if (!passwordHashingService.Verify(req.password, user.password_hash)) {
            return BadRequest(new BaseResponse<string>{
                Message = "Invalid credential",
            });
        }

        var token = tokenService.IssueToken(user.username);
        return Ok(new BaseResponse<string>{
            Data = token,
            Message = "Success",
        });
    }

    [HttpPost]
    [Route("signup")]
    public ActionResult<BaseResponse<string>> Signup(SignupRequest req)
    {
        try
        {
            var newUser = new UserModel()
            {
                email = req.email,
                username = req.username,
                password_hash = passwordHashingService.HashPassword(req.password),
            };

            userService.CreateUser(newUser);
            return StatusCode(StatusCodes.Status201Created, new BaseResponse<string>
            {
                Message = "Success",
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse<string>
            {
                Message = e.Message,
            });
        }
    }
}
