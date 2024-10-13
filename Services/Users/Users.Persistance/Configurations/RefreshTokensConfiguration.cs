using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Persistance.Entities;

namespace Users.Persistance.Configurations
{
    public  class RefreshTokensConfiguration: IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.HasKey(rf => rf.Id);
            builder.Property(rf => rf.ExpirationDate).IsRequired();
            builder.Property(rf => rf.UserEmail).IsRequired();
            builder.Property(rf => rf.Token).IsRequired();
            builder.Property(rf => rf.UserRole).IsRequired();
        }
    }
}