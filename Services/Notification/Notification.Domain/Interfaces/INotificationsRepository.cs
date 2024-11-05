using Notifications.Domain.Models;

namespace Notifications.Domain.Interfaces
{
    public interface INotificationsRepository
    {
        Task<Guid> Create(Notification notification);
        Task<Guid> Delete(Guid id);
        void DeleteAll();
        Task<List<Notification>> GetAll(int page, int pageSize);
        Task<List<Notification>> GetByUserId(Guid userId, int page, int pageSize);
        Task<Notification?> GetUserNotification(Guid userId, Guid notificationId);
    }
}
