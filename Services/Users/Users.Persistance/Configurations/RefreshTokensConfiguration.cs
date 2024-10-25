using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;

namespace Users.Persistance.Configurations
{
    public  class RefreshTokensConfiguration: IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserEmail)
                .IsRequired();
        }
        
    }
}