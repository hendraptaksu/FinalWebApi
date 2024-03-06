using FinalWebApi.API.Util;
using FinalWebApi.API.Models;

namespace FinalWebApi.API.Services.UserService;
public interface IUserService
{
    UserModel CreateUser(UserModel user);

    UserModel? GetUser(long id);

    UserModel? GetUser(string email);
}