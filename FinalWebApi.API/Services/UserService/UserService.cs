using FinalWebApi.API.Models;
using FinalWebApi.API.Database;
using FinalWebApi.API.Util;
using System.ComponentModel.DataAnnotations;

namespace FinalWebApi.API.Services.UserService;

public class UserService : IUserService
{
    private readonly AppDbContext dbContext;

    public UserService(AppDbContext c)
    {
        dbContext = c;
    }

    public UserModel CreateUser(UserModel user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        return user;
    }

    public UserModel? GetUser(long id)
    {
        return dbContext.Users.Where(u => u.id == id).FirstOrDefault();
    }


    public UserModel? GetUser(string email) {
        return dbContext.Users.Where(u => u.email == email).FirstOrDefault();
    }
}
