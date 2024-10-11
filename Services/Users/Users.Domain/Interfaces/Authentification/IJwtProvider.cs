using System.Security.Claims;
using Users.Domain.Models;

namespace Users.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(Claim[] claims);
        string GenerateRefreshToken();
    }
}