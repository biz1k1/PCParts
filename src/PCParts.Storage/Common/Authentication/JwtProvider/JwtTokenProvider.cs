using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using PCParts.Application.Abstraction.Authentication;

namespace PCParts.Storage.Common.Authentication.JwtProvider
{
    public class JwtTokenProvider(
        IConfiguration configuration):IJwtTokenProvider
    {
        public string Create(Guid id, string email)
        {
            string secretKey = configuration["Jwt:SecretKey"]!;
            var securiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securiryKey, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,email),
                    new Claim("email_verified", "false"),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpiresInMinutes"]!)
                ),
                SigningCredentials = credentials
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescription);

            return token;
        }
    }
}
