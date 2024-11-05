using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;
using Users.Domain.Models.AuthModels;
using Users.Persistance.Configurations;

namespace Users.Persistance
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public void EnsureSuperAdmin()
        {
            var superAdminId = Guid.Parse("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342");
            var superAdmin = Users.Find(superAdminId);

            if (superAdmin == null)
            {
                superAdmin = User.Create(
                    superAdminId,
                    "admin",
                    "admin",
                    DateOnly.Parse("2003-11-10"),
                    "admin@gmail.com",
                    BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                    "SuperAdmin"
                );
                Users.Add(superAdmin);
                SaveChanges();
            }
        }
    }
}
