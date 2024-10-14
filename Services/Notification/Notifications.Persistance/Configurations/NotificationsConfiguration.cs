using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifications.Persistance.Entities;

namespace Notifications.Persistance.Configurations
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(n => n.Message)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(u => u.DateTime).IsRequired();
            builder.Property(u => u.UserId).IsRequired();
        }

    }
}
