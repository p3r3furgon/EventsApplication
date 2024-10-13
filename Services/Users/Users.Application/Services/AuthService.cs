using System.Security.Claims;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;
using Users.Infrastructure;

namespace Users.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(IRefreshTokensRepository refreshTokensRepository, IJwtProvider jwtProvider,
            IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _refreshTokensRepository = refreshTokensRepository;
            _jwtProvider = jwtProvider;
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Register(string firstName, string surname, DateOnly birthDate, string email, string password)
        {
            string passwordHash = _passwordHasher.Generate(password);
            var users = await _usersRepository.Get();
            foreach (var us in users)
            {
                if (us.Email == email)
                    throw new Exception("This email is already used");
            }
            var user = User.Create(Guid.NewGuid(), firstName, surname, birthDate, email, passwordHash, "User");
            await _usersRepository.Create(user);
        }

        public async Task<(string, string)> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);
            var verifyResult = _passwordHasher.Verify(password, user.PasswordHash);
            if (!verifyResult)
            {
                throw new Exception("Failed to login");
            }

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];
            var accessToken = _jwtProvider.GenerateJwtToken(claims);
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            return (accessToken, refreshToken);

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
