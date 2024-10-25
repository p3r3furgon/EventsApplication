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
    }
}
