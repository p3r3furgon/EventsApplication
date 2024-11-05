using CommonFiles.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using Users.Application.Exceptions;
using Users.Domain.Interfaces;
using Users.Domain.Interfaces.Repositories;
using Users.Infrastructure;

namespace Users.Application.UseCases.AuthUseCases.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommandHandler
        : IRequestHandler<UpdateAccessTokenCommand, UpdateAccessTokenResponse>
    {

        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUsersRepository _usersRepository;

        public UpdateAccessTokenCommandHandler(IRefreshTokensRepository refreshTokensRepository,
            IJwtProvider jwtProvider, IOptions<JwtOptions> options, IUsersRepository usersRepository)
        {
            _refreshTokensRepository = refreshTokensRepository;
            _jwtProvider = jwtProvider;
            _usersRepository = usersRepository;
        }
        public async Task<UpdateAccessTokenResponse> Handle(UpdateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var storedRefreshToken = await _refreshTokensRepository.Get(WebUtility.UrlDecode(request.RefreshToken));

            if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid or expired refresh token");
            }

            var user = await _usersRepository.GetByEmail(storedRefreshToken.UserEmail.ToString());

            var newAccessToken = _jwtProvider.GenerateJwtToken(user);

            return new UpdateAccessTokenResponse(newAccessToken);
        }
    }
}
