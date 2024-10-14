using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Domain.Interfaces;
using System.Security.Claims;

namespace Notifications.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserNotifications([FromQuery] GridifyQuery gridifyQuery)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var notifications = await _notificationService.GetNotificationByUserId(Guid.Parse(userId));

            var result = notifications.AsQueryable()
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
            var notification = await _notificationService.GetUserNotificationById(notificationId, Guid.Parse(userId));
            return StatusCode(StatusCodes.Status200OK, notification);
        }

        [HttpDelete("{notificationId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserNotification(Guid notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            await _notificationService.DeleteNotification(notificationId, Guid.Parse(userId));

            return StatusCode(StatusCodes.Status200OK, notificationId);
        }

    }
}
