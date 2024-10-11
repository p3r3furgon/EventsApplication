using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Models;
using Users.Persistance.Entities;

namespace Users.Persistance.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly UsersDbContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(UsersDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<User>> Get()
        {
            var usersEntities = await _context.Users.ToListAsync();
            var users = _mapper.Map<List<User>>(usersEntities);
            return users;
        }

        public async Task<Guid> Create(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Update(Guid id, string firstName, string surname, DateTime birthDate, string email, string passwordHash, string role)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.FirstName, p => firstName)
                    .SetProperty(p => p.Surname, p => surname)
                    .SetProperty(p => p.BirthDate, p => birthDate)
                    .SetProperty(p => p.Email, p => email)
                    .SetProperty(p => p.PasswordHash, p => passwordHash)
                    .SetProperty(p => p.Role, p => role));
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();
            return _mapper.Map<User>(userEntity);
        }

    }
}
