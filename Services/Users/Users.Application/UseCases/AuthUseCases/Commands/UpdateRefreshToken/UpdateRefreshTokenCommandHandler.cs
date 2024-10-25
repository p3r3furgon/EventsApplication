using CommonFiles.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using Users.Domain.Interfaces.Repositories;
using Users.Infrastructure;

namespace Users.Application.UseCases.AuthUseCases.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommandHandler
        : IRequestHandler<UpdateRefreshTokenCommand, UpdateRefreshTokenResponse>
    {

        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly JwtOptions _options;
        private readonly IUsersRepository _usersRepository;
        public UpdateRefreshTokenCommandHandler(IRefreshTokensRepository refreshTokensRepository,
            IJwtProvider jwtProvider, IOptions<JwtOptions> options, IUsersRepository usersRepository)
        {
            _refreshTokensRepository = refreshTokensRepository;
            _jwtProvider = jwtProvider;
            _options = options.Value;
            _usersRepository = usersRepository;
        }
        public async Task<UpdateRefreshTokenResponse> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedRefreshToken = await _refreshTokensRepository.Get(WebUtility.UrlDecode(request.RefreshToken));

            if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token"); //401
            }

            var user = await _usersRepository.GetByEmail(storedRefreshToken.UserEmail.ToString());

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];

            var newAccessToken = _jwtProvider.GenerateJwtToken(claims);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken();
            await _refreshTokensRepository.Save(storedRefreshToken, newRefreshToken, DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays));

            return new UpdateRefreshTokenResponse(newAccessToken, newRefreshToken);
        }
    }
}
