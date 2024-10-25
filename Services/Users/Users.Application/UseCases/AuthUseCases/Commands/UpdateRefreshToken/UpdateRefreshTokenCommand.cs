using MediatR;

namespace Users.Application.UseCases.AuthUseCases.Commands.UpdateRefreshToken
{
    public record UpdateRefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<UpdateRefreshTokenResponse>;
    public record UpdateRefreshTokenResponse(string NewAccessToken, string NewRefreshToken);
}
