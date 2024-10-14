using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using AutoMapper;
using Notifications.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Notifications.Persistance.Repositories
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly NotificationsDbContext _context;
        private readonly IMapper _mapper;
        public NotificationsRepository(NotificationsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Guid> Create(Notification notification)
        {
            var notificationEntity = _mapper.Map<NotificationEntity>(notification);
            await _context.Notifications.AddAsync(notificationEntity);
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

        public async Task<List<Notification>> GetByUserId(Guid userId)
        {
            var notificationsEntities = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
            var notifications = _mapper.Map<List<Notification>>(notificationsEntities);
            return notifications;
        }
    }
}
