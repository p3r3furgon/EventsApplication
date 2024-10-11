using System.Security.Claims;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models.AuthModels;
using Users.Infrastructure;

namespace Users.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(IRefreshTokensRepository refreshTokensRepository, IJwtProvider jwtProvider)
        {
            _refreshTokensRepository = refreshTokensRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<RefreshToken> GetStoredRefreshToken(string refreshToken)
        {
            return await _refreshTokensRepository.Get(refreshToken);
        }

        public async Task CreateRefreshToken(RefreshToken refreshToken)
        {
            await _refreshTokensRepository.Create(refreshToken);
        }
        public async Task SaveRefreshToken(RefreshToken refreshToken, string newRefreshToken, DateTime expirationDate)
        {
            await _refreshTokensRepository.Save(refreshToken, newRefreshToken, expirationDate);
        }

        public string GenerateJwtToken(Claim[] claims)
        {
            return _jwtProvider.GenerateJwtToken(claims);
        }

        public string GenerateRefreshToken()
        {
            return _jwtProvider.GenerateRefreshToken();
        }
    }
}
