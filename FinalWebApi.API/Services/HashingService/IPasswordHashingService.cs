namespace FinalWebApi.API.Services.HashingService
{
    public interface IPasswordHashingService
    {
        string HashPassword(string plainText);

        bool Verify(string plainText, string hash);
    }
}