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
    }
}
