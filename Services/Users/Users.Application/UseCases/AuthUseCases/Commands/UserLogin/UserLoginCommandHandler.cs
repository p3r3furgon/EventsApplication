using CommonFiles.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Users.Application.Exceptions;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models.AuthModels;
using Users.Infrastructure;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    { 
        private readonly IUsersRepository _usersRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly JwtOptions _options;

        public UserLoginCommandHandler(IUsersRepository usersRepository, IPasswordHasher passwordHasher, 
            IRefreshTokensRepository refreshTokensRepository, IJwtProvider jwtProvider, IOptions<JwtOptions> options)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _refreshTokensRepository = refreshTokensRepository;
            _jwtProvider = jwtProvider;
            _options = options.Value;
        }
        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            UserLoginCommandValidator validator = new();
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                throw new BadRequestException(results.Errors);
            }

            var user = await _usersRepository.GetByEmail(request.Email);
            if (user == null)
            {
                throw new UserNotFoundException("User with such email doesnt exist");
            }

            var verifyResult = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!verifyResult)
            {
                throw new BadRequestException("Failed to login");
            }

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];

            var accessToken = _jwtProvider.GenerateJwtToken(claims);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            await _refreshTokensRepository.Create(new RefreshToken(Guid.NewGuid(), refreshToken, request.Email, DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)));

            return new UserLoginResponse(accessToken, refreshToken);
        }
    }
}
