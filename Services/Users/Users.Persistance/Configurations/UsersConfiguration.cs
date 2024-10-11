using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;
using Users.Persistance.Entities;

namespace Users.Persistance.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasOne<RefreshTokenEntity>()
            .WithOne()
            .HasForeignKey<RefreshTokenEntity>(e => e.UserEmail)
            .IsRequired();

            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName)
                .HasMaxLength(User.NAMES_MAX_LENGTH)
                .IsRequired();

            builder.Property(u => u.Surname)
                .HasMaxLength(User.NAMES_MAX_LENGTH)
                .IsRequired();

            builder.Property(u => u.BirthDate).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired();
        }
    }
}
