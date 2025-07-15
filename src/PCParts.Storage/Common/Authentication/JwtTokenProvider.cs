using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PCParts.Application.Abstraction.Authentication;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Common.Authentication
{
    public class JwtTokenProvider(
        IConfiguration configuration):IJwtTokenProvider
    {
        public string Create(User user)
        {
            string secretKey = configuration["Jwt:SecretKey"]!;
            var securiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securiryKey, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim("email_verified", user.Email)
                ])
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescription);

            return token;
        }
    }
}
