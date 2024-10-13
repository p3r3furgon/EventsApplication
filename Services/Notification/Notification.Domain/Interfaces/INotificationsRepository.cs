using Notifications.Domain.Models;

namespace Notifications.Domain.Interfaces
{
    public interface INotificationsRepository
    {
        Task<Guid> Create(Notification notification);
        Task<Guid> Delete(Guid id);
        Task<List<Notification>> GetByUserId(Guid userId);
    }
}
