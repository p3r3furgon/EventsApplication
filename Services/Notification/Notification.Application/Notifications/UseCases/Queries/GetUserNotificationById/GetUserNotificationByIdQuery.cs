using MediatR;
using Notifications.Application.Dtos;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotificationById
{
    public record GetUserNotificationByIdQuery(Guid UserId, Guid NotificationId) : IRequest<GetUserNotificationByIdResponse>;

    public record GetUserNotificationByIdResponse(NotificationResponseDto Notification);
}
