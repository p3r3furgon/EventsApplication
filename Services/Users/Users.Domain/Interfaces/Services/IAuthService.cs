using System.Security.Claims;
using Users.Domain.Models.AuthModels;

namespace Users.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(Claim[] claims);
        string GenerateRefreshToken();
        Task<RefreshToken> GetStoredRefreshToken(string refreshToken);
        Task SaveRefreshToken(RefreshToken refreshToken, string newRefreshToken, DateTime expirationDate);
        Task CreateRefreshToken(RefreshToken refreshToken);
    }
}
