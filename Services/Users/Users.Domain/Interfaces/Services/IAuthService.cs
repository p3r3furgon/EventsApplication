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
        Task Register(string firstName, string surname, DateOnly birthDate, string email, string password);
        Task<(string, string)> Login(string email, string password);
    }
}
