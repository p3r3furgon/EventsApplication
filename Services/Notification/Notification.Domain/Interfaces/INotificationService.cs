using Notifications.Domain.Models;

namespace Notifications.Domain.Interfaces
{
    public interface INotificationService
    {
        Task<Guid> DeleteNotification(Guid notificationId, Guid userId);
        Task<List<Notification>> GetNotificationByUserId(Guid userId);
        Task<Notification> GetUserNotificationById(Guid notificationId, Guid userId);
    }
}
