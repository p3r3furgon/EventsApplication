using CommonFiles.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Users.Domain.Models;

namespace Users.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateJwtToken(User user)
        {

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(_options.AccessTokenExpirationMinutes),
                claims: claims);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
