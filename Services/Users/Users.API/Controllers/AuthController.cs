using CommonFiles.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using Users.API.Dtos;
using Users.API.Validators;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models.AuthModels;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsersService _usersService;
        private readonly JwtOptions _options;

        public AuthController(IAuthService authService, IOptions<JwtOptions> options, IUsersService usersService)
        {
            _authService = authService;
            _usersService = usersService;
            _options = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            RegisterRequestValidator validator = new();
            var results = validator.Validate(registerRequest);
            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            await _authService.Register(registerRequest.FirstName, registerRequest.Surname,
                DateOnly.Parse(registerRequest.BirthDate), registerRequest.Email, registerRequest.Password);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginRequestValidator validator = new();
            var results = validator.Validate(loginRequest);
            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            var tokens = await _authService.Login(loginRequest.Email, loginRequest.Password);
            var accessToken = tokens.Item1;
            var refreshToken = tokens.Item2;

            await _authService.CreateRefreshToken(new RefreshToken(Guid.NewGuid(), refreshToken, loginRequest.Email, DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)));

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)
            });

            var responce = new Dictionary<string, string> { };
            responce["access_token"] = accessToken;
            responce["refresh_token"] = refreshToken;
            return StatusCode(StatusCodes.Status200OK, responce);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var storedRefreshToken = await _authService.GetStoredRefreshToken(WebUtility.UrlDecode(request.RefreshToken));

            if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid or expired refresh token.");
            }

            var user = await _usersService.GetUserByEmail(storedRefreshToken.UserEmail.ToString());

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];

            var newAccessToken = _authService.GenerateJwtToken(claims);
            var newRefreshToken = _authService.GenerateRefreshToken();

            await _authService.SaveRefreshToken(storedRefreshToken, newRefreshToken, DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays));

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)
            });

            var responce = new Dictionary<string, string> { };
            responce["access_token"] = newAccessToken;
            responce["refresh_token"] = newRefreshToken;

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }
    }
}
