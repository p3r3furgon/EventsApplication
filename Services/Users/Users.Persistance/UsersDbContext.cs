using Microsoft.EntityFrameworkCore;
using Users.Persistance.Entities;

namespace Users.Persistance
{
    public class UsersDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = Guid.Parse("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                    FirstName = "admin",
                    Surname = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                    BirthDate = DateOnly.Parse("2003-11-10"),
                    Role = "SuperAdmin"
                }
            );

        }
    }
}
