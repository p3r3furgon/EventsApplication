using Events.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Persistance.Configurations
{
    public class ParticipantConfiguration: IEntityTypeConfiguration<ParticipantEntity>
    {
        public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
        {

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
