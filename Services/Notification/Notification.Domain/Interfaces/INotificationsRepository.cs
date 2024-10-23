using Notifications.Domain.Models;

namespace Notifications.Domain.Interfaces
{
    public interface INotificationsRepository
    {
        Task<Guid> Create(Notification notification);
        Task<Guid> Delete(Guid id);
        Task DeleteAll();
        Task<List<Notification>> GetAll();
        Task<List<Notification>> GetByUserId(Guid userId);
    }
}
