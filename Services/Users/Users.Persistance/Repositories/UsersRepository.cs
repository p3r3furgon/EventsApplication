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

        public async Task<User> GetById(Guid id)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
                throw new Exception("There is no user with this id");
            var user = _mapper.Map<User>(userEntity);
            return user;
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

        public async Task<Guid> Update(Guid id, string? firstName, string? surname, DateOnly? birthDate, string? email, string? passwordHash, string? role)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
                throw new Exception("There is no user with this id");

            userEntity.FirstName = (string.IsNullOrEmpty(firstName)) ? userEntity.FirstName : firstName;
            userEntity.Surname = (string.IsNullOrEmpty(surname)) ? userEntity.Surname : surname;
            userEntity.BirthDate = birthDate ?? userEntity.BirthDate;
            userEntity.Email = (string.IsNullOrEmpty(email)) ? userEntity.Email : email;
            userEntity.PasswordHash = (string.IsNullOrEmpty(passwordHash)) ? userEntity.PasswordHash : passwordHash;
            userEntity.Role = (string.IsNullOrEmpty(role)) ? userEntity.Role : role;
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("There is no user with this email");
            return _mapper.Map<User>(userEntity);
        }

    }
}
