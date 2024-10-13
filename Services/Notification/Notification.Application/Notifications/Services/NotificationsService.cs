using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.Services
{
    public class NotificationsService : INotificationService
    {
        private readonly INotificationsRepository _notificationRepository;

        public NotificationsService(INotificationsRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Guid> DeleteNotification(Guid notificationId, Guid userId)
        {
            var userNotifications = await _notificationRepository.GetByUserId(userId);
            var userNotification = userNotifications.Find(n => n.Id == notificationId);

            if (userNotification == null)
                throw new Exception("There is no notification with this id");

            return await _notificationRepository.Delete(notificationId);
        }

        public async Task<List<Notification>> GetNotificationByUserId(Guid userId)
        {
            return await _notificationRepository.GetByUserId(userId);
        }

        public async Task<Notification> GetUserNotificationById(Guid notificationId, Guid userId)
        {
            var userNotifications = await _notificationRepository.GetByUserId(userId);
            var userNotification = userNotifications.Find(n => n.Id == notificationId);

            if (userNotification == null)
                throw new Exception("There is no notification with this id");

            return userNotification;
        }
    }
}
