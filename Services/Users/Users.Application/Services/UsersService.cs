using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models;

namespace Users.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<Guid> UpdateUser(Guid id, string firstName, string surname, DateOnly? birthDate, string email, string password, string role)
        {
            var passwordHash = string.IsNullOrEmpty(password) ? "" : _passwordHasher.Generate(password);
            return await _usersRepository.Update(id, firstName, surname, birthDate, email, passwordHash, role);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }
    }
}
