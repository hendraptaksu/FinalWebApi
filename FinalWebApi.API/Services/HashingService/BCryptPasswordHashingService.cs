using BC = BCrypt.Net.BCrypt;

namespace FinalWebApi.API.Services.HashingService;

public class BCryptPasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string plainText)
    {
        return BC.HashPassword(plainText);
    }

    public bool Verify(string plainText, string hash)
    {
        return BC.Verify(plainText, hash);
    }
}
