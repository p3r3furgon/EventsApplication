using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Users.Domain.Models;
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

            builder.HasData(
                new UserEntity
                {
                    Id = Guid.Parse("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                    FirstName = "admin",
                    Surname = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                    BirthDate = DateOnly.Parse("2003-11-10"),
                    Role = "SuperAdmin"
                });
        }
    }
}
