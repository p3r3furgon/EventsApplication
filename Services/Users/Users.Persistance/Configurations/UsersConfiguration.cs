using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;
namespace Users.Persistance.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        { 
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName)
                .HasMaxLength(User.NAMES_MAX_LENGTH)
                .IsRequired();

            builder.Property(u => u.Surname)
                .HasMaxLength(User.NAMES_MAX_LENGTH)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.BirthDate).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            builder.HasData(
                User.Create(
                     Guid.Parse("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                     "admin",
                     "admin",
                     DateOnly.Parse("2003-11-10"),
                     "admin@gmail.com",
                     BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                     "SuperAdmin"
                    )
                );
        }
    }
}
