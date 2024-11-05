using CommonFiles.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.UseCases.UserUseCases.Commands.DeleteUser;
using Users.Application.UseCases.UserUseCases.Commands.GrantAdminRole;
using Users.Application.UseCases.UserUseCases.Commands.RevokeAdminRole;
using Users.Application.UseCases.UserUseCases.Commands.UpdateUser;
using Users.Application.UseCases.UserUseCases.Queries.GetUserById;
using Users.Application.UseCases.UserUseCases.Queries.GetUsers;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            var response = await _mediator.Send(new GetUsersQuery(paginationParams));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery(id));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserCommand updateUserCommand)
        {
            var response = await _mediator.Send(updateUserCommand);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _mediator.Send(new DeleteUserCommand(id));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("{id}/assign-admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> GrantAdminRole(Guid id)
        {
            var response = await _mediator.Send(new GrantAdminRoleCommand(id));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{id}/remove-admin")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> RevokeAdminRole(Guid id)
        {
            var response = await _mediator.Send(new RevokeAdminRoleCommand(id));
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
