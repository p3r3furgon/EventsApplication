using Events.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistance.Configurations
{
    public class EventsConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Place)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.Property(e => e.Category)
                .HasMaxLength(50);


            builder.Property(e => e.DateTime).IsRequired();
            builder.Property(e => e.MaxParticipantNumber).IsRequired();

            builder
                .HasMany(e => e.Participants)
                .WithOne(e => e.Event)
                .HasForeignKey("EventId")
                .IsRequired(false);
        }
    }
}
