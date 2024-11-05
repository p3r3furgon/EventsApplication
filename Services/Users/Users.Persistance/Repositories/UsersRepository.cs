using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models;
namespace Users.Persistance.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Get(int pageNumber, int pageSize) => 
            await _context.Users
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        public async Task<User> GetById(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<User> GetByEmail(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task Create(User user) =>
            await _context.Users.AddAsync(user);

        public async Task Delete(Guid id) => 
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

        public void Update(User user) => 
            _context.Users.Update(user);
    }
}
