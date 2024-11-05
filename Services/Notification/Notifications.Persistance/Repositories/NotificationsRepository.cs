using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Notifications.Persistance.Repositories
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly NotificationsDbContext _context;
        public NotificationsRepository(NotificationsDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            return notification.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var notification = await _context.Notifications
                .FindAsync(id);

            _context.Remove(notification);
            return id;
        }

        public void DeleteAll()
        { 
            var notificationsEntities = _context.Notifications;
            foreach (var notification in notificationsEntities)
            {
                _context.Notifications.Remove(notification);
            }
        }

        public async Task<List<Notification>> GetAll(int page, int pageSize) => 
            await _context.Notifications
            .OrderBy(n => n.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<List<Notification>> GetByUserId(Guid userId, int page, int pageSize) => 
            await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderBy(n => n.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<Notification?> GetUserNotification(Guid userId, Guid notificationId) =>
            await  _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
    }
}
