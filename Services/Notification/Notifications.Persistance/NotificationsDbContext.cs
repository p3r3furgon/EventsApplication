using Microsoft.EntityFrameworkCore;
using Notifications.Domain.Models;

namespace Notifications.Persistance
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Notification> Notifications { get; set; }
    }
}

