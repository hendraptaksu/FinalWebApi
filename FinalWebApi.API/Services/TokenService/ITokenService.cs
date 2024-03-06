namespace FinalWebApi.API.Services.TokenService
{
    public interface ITokenService
    {
        
        /// <summary>
        /// Issues a auth token that later is used to request restricted endpoints
        /// </summary>
        /// <param name="name">name token claim</param>
        /// <returns></returns>
        string IssueToken(string name);
    }
}

