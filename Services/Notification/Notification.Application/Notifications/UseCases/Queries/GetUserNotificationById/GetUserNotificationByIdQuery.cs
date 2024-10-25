using MediatR;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotificationById
{
    public record GetUserNotificationByIdQuery(Guid UserId, Guid NotificationId) : IRequest<GetUserNotificationByIdResponse>;

    public record GetUserNotificationByIdResponse(Notification Notification);
}
