using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace FinalWebApi.API.Services.TokenService
{
    // Dev Note: actually I was naming this class JwtService and filename JwtService.cs. 
    // But somehow the IDE feature doesn't work. Like linting is strangely inactive.
    // Soon after I change the file name to something else (e.g. TokenService.cs), linting starts working.
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration c)
        {
            this.config = c;
        }

        public string IssueToken(string name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>(){
                new("name", name),
            };

            var Sectoken = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }
    }

}
