using Gridify;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Notifications.UseCases.Commands.DeleteAllNotifications;
using Notifications.Application.Notifications.UseCases.Commands.DeleteNotification;
using Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications;
using Notifications.Application.Notifications.UseCases.Queries.GetUserNotificationById;
using Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications;
using System.Security.Claims;

namespace Notifications.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserNotifications([FromQuery] GridifyQuery gridifyQuery)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var response = await _mediator.Send(new GetUserNotificationsQuery(Guid.Parse(userId)));
           
            var result = response.Notifications.AsQueryable()
                          .ApplyFiltering(gridifyQuery)
                          .ApplyOrdering(gridifyQuery)
                          .ApplyPaging(gridifyQuery.Page, gridifyQuery.PageSize);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("{notificationId}")]
        [Authorize]
        public async Task<IActionResult> GetUserNotificationById(Guid notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var response = await _mediator.Send(new GetUserNotificationByIdQuery(Guid.Parse(userId), notificationId));
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{notificationId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserNotification(Guid notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var response = await _mediator.Send(new DeleteNotificationCommand(Guid.Parse(userId), notificationId));

            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpDelete("all")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            var response = await _mediator.Send(new DeleteNotificationsCommand());
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("all")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var response = await _mediator.Send(new GetAllNotificationsQuery());
            return StatusCode(StatusCodes.Status200OK, response);
        }

    }
}
