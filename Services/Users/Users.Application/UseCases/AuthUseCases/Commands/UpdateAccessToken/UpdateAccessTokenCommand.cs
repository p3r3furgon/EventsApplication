using MediatR;

namespace Users.Application.UseCases.AuthUseCases.Commands.UpdateAccessToken
{
    public record UpdateAccessTokenCommand(string AccessToken, string RefreshToken) : IRequest<UpdateAccessTokenResponse>;
    public record UpdateAccessTokenResponse(string NewAccessToken);
}
