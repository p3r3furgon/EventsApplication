using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using Users.API.Dtos;
using Users.API.Validators;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models.AuthModels;
using Users.Infrastructure;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;
        private readonly JwtOptions _options;
        public UsersController(IUsersService usersService, IAuthService authService, IOptions<JwtOptions> options)
        {
            _usersService = usersService;
            _authService = authService;
            _options = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            RegisterRequestValidator validator = new();
            var results = validator.Validate(registerRequest);
            if(!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            await _usersService.Register(registerRequest.FirstName, registerRequest.Surname,
                registerRequest.BirthDate, registerRequest.Email, registerRequest.Password);
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

            var tokens = await _usersService.Login(loginRequest.Email, loginRequest.Password);
            var accessToken = tokens.Item1;
            var refreshToken = tokens.Item2;

            await _authService.CreateRefreshToken(new RefreshToken(Guid.NewGuid(), refreshToken, loginRequest.Email,
                "User", DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)));

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
                return Unauthorized("Invalid or expired refresh token.");
            }

            Claim[] claims = [new("userEmail", storedRefreshToken.UserEmail.ToString()), new(ClaimTypes.Role, storedRefreshToken.UserRole)];

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersService.GetUsers();
            return StatusCode(StatusCodes.Status200OK, users);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest updateUser)
        {
            UpdateUserRequestValidator validator = new();
            var results = validator.Validate(updateUser);
            if (!results.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, results.Errors);
            }

            var userId = await _usersService.UpdateUser(id, updateUser.FirstName, updateUser.Surname,
                updateUser.BirthDate, updateUser.Email, updateUser.Password, updateUser.Role);
            return StatusCode(StatusCodes.Status200OK, userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = await _usersService.DeleteUser(id);
            return StatusCode(StatusCodes.Status200OK, userId);
        }

        

    }
}
