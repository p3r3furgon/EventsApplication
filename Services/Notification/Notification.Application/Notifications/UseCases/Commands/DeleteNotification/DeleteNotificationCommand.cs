using MediatR;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteNotification
{
    public record DeleteNotificationCommand(Guid UserId, Guid NotificationId) 
        : IRequest<DeleteNotificationResponse>;
    public record DeleteNotificationResponse(Guid Id);

}
