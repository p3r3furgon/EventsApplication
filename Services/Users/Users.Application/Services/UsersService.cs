using System.Security.Claims;
using System.Security.Principal;
using Users.Domain.Interfaces.Authentification;
using Users.Domain.Interfaces.Repositories;
using Users.Domain.Interfaces.Services;
using Users.Domain.Models;
using Users.Infrastructure;

namespace Users.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<Guid> CreateUser(string firstName, string surname, DateTime birthDate, string email, string password, string role)
        {
            string passwordHash = _passwordHasher.Generate(password);
            var user = new User(Guid.NewGuid(), firstName, surname, birthDate, email, passwordHash, role);
            return await _usersRepository.Create(user);
        }

        public async Task<Guid> UpdateUser(Guid id, string firstName, string surname, DateTime birthDate, string email, string password, string role)
        {
            return await _usersRepository.Update(id, firstName, surname, birthDate, email, _passwordHasher.Generate(password), role);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }

        public async Task Register(string firstName, string surname, DateTime birthDate, string email, string password)
        {
            string passwordHash = _passwordHasher.Generate(password);
            var users = await _usersRepository.Get();
            foreach(var us in users )
            {
                if (us.Email == email)
                    throw new Exception("This email is already used");
            }
            var user = new User(Guid.NewGuid(), firstName, surname, birthDate, email, passwordHash, "User");
            await _usersRepository.Create(user);
        }

        public async Task<(string, string)> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);
            var verifyResult = _passwordHasher.Verify(password, user.PasswordHash);
            if (!verifyResult)
            {
                throw new Exception("Failed to login");
            }

            Claim[] claims = [ new(ClaimTypes.PrimarySid, user.Id.ToString()), new(ClaimTypes.Role, user.Role), 
                new(ClaimTypes.Name, user.FirstName), new(ClaimTypes.Surname, user.Surname), new(ClaimTypes.Email, user.Email)];
            var accessToken = _jwtProvider.GenerateJwtToken(claims);
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            return (accessToken, refreshToken);

        }
    }
}
