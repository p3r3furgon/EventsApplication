using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Users.API.Dtos;
using Users.API.Validators;
using Users.Domain.Interfaces.Services;
using Users.Infrastructure;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, IAuthService authService, IOptions<JwtOptions> options)
        {
            _usersService = usersService;
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
