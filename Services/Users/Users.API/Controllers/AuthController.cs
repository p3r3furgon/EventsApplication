using CommonFiles.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Users.Application.UseCases.AuthUseCases.Commands.UpdateAccessToken;
using Users.Application.UseCases.AuthUseCases.Commands.UserLogin;
using Users.Application.UseCases.AuthUseCases.Commands.UserRegister;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtOptions _options;

        public AuthController(IMediator mediator, IOptions<JwtOptions> options)
        {
            _mediator = mediator;
            _options = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterCommand userRegisterCommand)
        {
            var response = await _mediator.Send(userRegisterCommand);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand userLoginCommand)
        {
            var response = await _mediator.Send(userLoginCommand);

            Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)
            });

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] UpdateAccessTokenCommand updateRefreshTokenCommand)
        {
            var response = await _mediator.Send(updateRefreshTokenCommand);

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
