using System.Security.Principal;
using Users.Domain.Models;

namespace Users.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task<Guid> CreateUser(string firstName, string surname, DateTime birthDate, string email, string password, string role);
        Task<Guid> DeleteUser(Guid id);
        Task<ICollection<User>> GetUsers();
        Task<(string, string)> Login(string email, string password);
        Task Register(string firstName, string surname, DateTime birthDate, string email, string password);
        Task<Guid> UpdateUser(Guid id, string firstName, string surname, DateTime birthDate, string email, string password, string role);
    }
}
