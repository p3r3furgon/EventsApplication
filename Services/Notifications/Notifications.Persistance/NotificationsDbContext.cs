using Microsoft.EntityFrameworkCore;
using Notifications.Persistance.Entities;

namespace Notifications.Persistance
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }
        public DbSet<NotificationEntity> Notifications { get; set; }
    }
}

