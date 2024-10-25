using MediatR;
using Notifications.Application.Exceptions;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, DeleteNotificationResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        public DeleteNotificationCommandHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }
        public async Task<DeleteNotificationResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var userNotifications = await _notificationsRepository.GetByUserId(request.UserId);
            var userNotification = userNotifications.Find(n => n.Id == request.NotificationId);

            if (userNotification == null)
                throw new NotificationNotFoundException(request.NotificationId);

            await _notificationsRepository.Delete(request.NotificationId);
            return new DeleteNotificationResponse(userNotification.Id);
        }
    }
}
