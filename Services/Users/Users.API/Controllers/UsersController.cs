using CommonFiles.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Users.API.Dtos;
using Users.API.Validators;
using Users.Domain.Interfaces.Services;

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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _usersService.GetUserById(id);
            return StatusCode(StatusCodes.Status200OK, user);
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
                DateOnly.Parse(updateUser.BirthDate), updateUser.Email, updateUser.Password, "");
            return StatusCode(StatusCodes.Status200OK, userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = await _usersService.DeleteUser(id);
            return StatusCode(StatusCodes.Status200OK, userId);
        }

        [HttpPut("{id}/assign-admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> GrantAdminRole(Guid id)
        {
            var user = await _usersService.GetUserById(id);
            
            if(user.Role == "Admin")
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User is already admin");
            }

            await _usersService.UpdateUser(
                id: id,
                firstName: "",
                surname: "",
                birthDate: null,
                email: "",
                password: "",
                role: "Admin"
                );

            return StatusCode(StatusCodes.Status200OK, id);
        }

        [HttpDelete("{id}/remove-admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> RevokeAdminRole(Guid id)
        {
            var user = await _usersService.GetUserById(id);

            if (user.Role != "Admin")
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User isnt admin");
            }

            await _usersService.UpdateUser(
                id: id,
                firstName: "",
                surname: "",
                birthDate: null,
                email: "",
                password: "",
                role: "User"
                );

            return StatusCode(StatusCodes.Status200OK, id);
        }

    }
}
