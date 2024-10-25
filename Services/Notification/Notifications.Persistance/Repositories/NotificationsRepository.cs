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
            await _context.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Notifications
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }

        public async Task DeleteAll()
        { 
            var notificationsEntities = _context.Notifications;
            foreach (var notification in notificationsEntities)
            {
                _context.Notifications.Remove(notification);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetAll() => 
            await _context.Notifications.ToListAsync();

        public async Task<List<Notification>> GetByUserId(Guid userId) => 
            await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
    }
}
